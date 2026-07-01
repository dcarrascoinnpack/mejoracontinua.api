using Dapper;
using MejoraContinua.Api.Data;
using MejoraContinua.Api.Models;

namespace MejoraContinua.Api.Repositories;

public class AnalisisCausaRaizRepository
{
    private readonly DbConnectionFactory _factory;

    public AnalisisCausaRaizRepository(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<bool> ExisteNoConformidad(int noConformidadId)
    {
        using var connection = _factory.CreateConnection();

        const string sql = "SELECT COUNT(1) FROM nc_no_conformidades WHERE id = @Id";

        var count = await connection.ExecuteScalarAsync<int>(sql, new { Id = noConformidadId });

        return count > 0;
    }

    public async Task<AnalisisCausaRaiz?> ObtenerPorNoConformidadId(int noConformidadId)
    {
        using var connection = _factory.CreateConnection();

        const string sql = @"
            SELECT
                id,
                no_conformidad_id AS NoConformidadId,
                metodologia,
                problema_detectado AS ProblemaDetectado,
                porque_1 AS Porque1,
                porque_2 AS Porque2,
                porque_3 AS Porque3,
                porque_4 AS Porque4,
                porque_5 AS Porque5,
                causa_raiz AS CausaRaiz,
                conclusion,
                creado_por AS CreadoPor,
                fecha_creacion AS FechaCreacion,
                actualizado_por AS ActualizadoPor,
                fecha_actualizacion AS FechaActualizacion
            FROM nc_analisis_causa_raiz
            WHERE no_conformidad_id = @NoConformidadId";

        return await connection.QueryFirstOrDefaultAsync<AnalisisCausaRaiz>(
            sql,
            new { NoConformidadId = noConformidadId });
    }

    public async Task<int> Crear(int noConformidadId, CrearAnalisisRequest request)
    {
        using var connection = _factory.CreateConnection();

        const string sql = @"
            INSERT INTO nc_analisis_causa_raiz
            (
                no_conformidad_id,
                metodologia,
                problema_detectado,
                porque_1,
                porque_2,
                porque_3,
                porque_4,
                porque_5,
                causa_raiz,
                conclusion,
                creado_por
            )
            VALUES
            (
                @NoConformidadId,
                @Metodologia,
                @ProblemaDetectado,
                @Porque1,
                @Porque2,
                @Porque3,
                @Porque4,
                @Porque5,
                @CausaRaiz,
                @Conclusion,
                @CreadoPor
            );

            SELECT LAST_INSERT_ID();";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                NoConformidadId = noConformidadId,
                request.Metodologia,
                request.ProblemaDetectado,
                request.Porque1,
                request.Porque2,
                request.Porque3,
                request.Porque4,
                request.Porque5,
                request.CausaRaiz,
                request.Conclusion,
                request.CreadoPor,
            });
    }

    public async Task<bool> Actualizar(int noConformidadId, CrearAnalisisRequest request)
    {
        using var connection = _factory.CreateConnection();

        const string sql = @"
            UPDATE nc_analisis_causa_raiz
            SET
                metodologia = @Metodologia,
                problema_detectado = @ProblemaDetectado,
                porque_1 = @Porque1,
                porque_2 = @Porque2,
                porque_3 = @Porque3,
                porque_4 = @Porque4,
                porque_5 = @Porque5,
                causa_raiz = @CausaRaiz,
                conclusion = @Conclusion,
                actualizado_por = @ActualizadoPor,
                fecha_actualizacion = UTC_TIMESTAMP()
            WHERE no_conformidad_id = @NoConformidadId";

        var filas = await connection.ExecuteAsync(
            sql,
            new
            {
                NoConformidadId = noConformidadId,
                request.Metodologia,
                request.ProblemaDetectado,
                request.Porque1,
                request.Porque2,
                request.Porque3,
                request.Porque4,
                request.Porque5,
                request.CausaRaiz,
                request.Conclusion,
                request.ActualizadoPor,
            });

        return filas > 0;
    }
}
