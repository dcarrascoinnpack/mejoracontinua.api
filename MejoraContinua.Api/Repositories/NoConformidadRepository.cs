using Dapper;
using MejoraContinua.Api.Data;
using MejoraContinua.Api.Models;


namespace MejoraContinua.Api.Repositories;

public class NoConformidadRepository
{
    private readonly DbConnectionFactory _factory;

    public NoConformidadRepository(
        DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<NoConformidad>> ObtenerTodas()
    {
        using var connection = _factory.CreateConnection();

        const string sql = @"
            SELECT
                id,
                codigo,
                tipo,
                origen,
                titulo,
                descripcion,
                severidad,
                estado,
                proceso,
                norma,
                fecha_creacion AS FechaCreacion
            FROM nc_no_conformidades
            ORDER BY id DESC";

        return await connection.QueryAsync<NoConformidad>(sql);
    }
    public async Task<int> Crear(CreateNoConformidadRequest request)
{
    using var connection = _factory.CreateConnection();

    var anio = DateTime.Now.Year;

    var ultimoCodigo = await connection.QueryFirstOrDefaultAsync<string>(
        @"SELECT codigo
          FROM nc_no_conformidades
          WHERE codigo LIKE @prefijo
          ORDER BY id DESC
          LIMIT 1",
        new
        {
            prefijo = $"NC-{anio}-%"
        });

    int correlativo = 1;

    if (!string.IsNullOrWhiteSpace(ultimoCodigo))
    {
        var partes = ultimoCodigo.Split('-');

        if (partes.Length == 3 &&
            int.TryParse(partes[2], out var ultimo))
        {
            correlativo = ultimo + 1;
        }
    }

    var codigo = $"NC-{anio}-{correlativo:D3}";

    const string sql = @"
        INSERT INTO nc_no_conformidades
        (
            codigo,
            tipo,
            origen,
            titulo,
            descripcion,
            severidad,
            estado,
            proceso,
            norma,
            fecha_deteccion,
            reportado_por,
            responsable
        )
        VALUES
        (
            @Codigo,
            @Tipo,
            @Origen,
            @Titulo,
            @Descripcion,
            @Severidad,
            'ABIERTA',
            @Proceso,
            @Norma,
            @FechaDeteccion,
            @ReportadoPor,
            @Responsable
        );

        SELECT LAST_INSERT_ID();";

    var id = await connection.ExecuteScalarAsync<int>(
        sql,
        new
        {
            Codigo = codigo,
            request.Tipo,
            request.Origen,
            request.Titulo,
            request.Descripcion,
            request.Severidad,
            request.Proceso,
            request.Norma,
            request.FechaDeteccion,
            request.ReportadoPor,
            request.Responsable
        });

    return id;
}
public async Task<NoConformidad?> ObtenerPorId(int id)
{
    using var connection = _factory.CreateConnection();

    const string sql = @"
        SELECT
            id,
            codigo,
            tipo,
            origen,
            titulo,
            descripcion,
            severidad,
            estado,
            proceso,
            norma,
            fecha_creacion AS FechaCreacion
        FROM nc_no_conformidades
        WHERE id = @Id";

    return await connection.QueryFirstOrDefaultAsync<NoConformidad>(
        sql,
        new { Id = id });
}
public async Task<bool> Actualizar(
    int id,
    CreateNoConformidadRequest request)
{
    using var connection = _factory.CreateConnection();

    const string sql = @"
        UPDATE nc_no_conformidades
        SET
            tipo = @Tipo,
            origen = @Origen,
            titulo = @Titulo,
            descripcion = @Descripcion,
            severidad = @Severidad,
            proceso = @Proceso,
            norma = @Norma,
            responsable = @Responsable
        WHERE id = @Id";

    var filas = await connection.ExecuteAsync(
        sql,
        new
        {
            Id = id,
            request.Tipo,
            request.Origen,
            request.Titulo,
            request.Descripcion,
            request.Severidad,
            request.Proceso,
            request.Norma,
            request.Responsable
        });

    return filas > 0;
}
}
