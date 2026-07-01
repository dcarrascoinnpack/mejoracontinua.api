using Dapper;
using MejoraContinua.Api.Data;
using MejoraContinua.Api.Models;

namespace MejoraContinua.Api.Repositories;

public class AccionCorrectivaRepository
{
    private readonly DbConnectionFactory _factory;

    public AccionCorrectivaRepository(DbConnectionFactory factory)
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

    private const string SelectColumnas = @"
        SELECT
            id,
            no_conformidad_id AS NoConformidadId,
            analisis_id AS AnalisisId,
            descripcion,
            responsable,
            fecha_limite AS FechaLimite,
            prioridad,
            estado,
            integracion_tareas_estado AS IntegracionTareasEstado,
            tarea_id AS TareaId,
            creado_por AS CreadoPor,
            fecha_creacion AS FechaCreacion,
            actualizado_por AS ActualizadoPor,
            fecha_actualizacion AS FechaActualizacion
        FROM nc_acciones_correctivas";

    public async Task<IEnumerable<AccionCorrectiva>> ObtenerPorNoConformidadId(int noConformidadId)
    {
        using var connection = _factory.CreateConnection();

        var sql = $"{SelectColumnas} WHERE no_conformidad_id = @NoConformidadId ORDER BY id DESC";

        return await connection.QueryAsync<AccionCorrectiva>(
            sql,
            new { NoConformidadId = noConformidadId });
    }

    public async Task<AccionCorrectiva?> ObtenerPorId(int id)
    {
        using var connection = _factory.CreateConnection();

        var sql = $"{SelectColumnas} WHERE id = @Id";

        return await connection.QueryFirstOrDefaultAsync<AccionCorrectiva>(sql, new { Id = id });
    }

    public async Task<int> Crear(int noConformidadId, CrearAccionCorrectivaRequest request)
    {
        using var connection = _factory.CreateConnection();

        const string sql = @"
            INSERT INTO nc_acciones_correctivas
            (
                no_conformidad_id,
                analisis_id,
                descripcion,
                responsable,
                fecha_limite,
                prioridad,
                estado,
                integracion_tareas_estado,
                creado_por
            )
            VALUES
            (
                @NoConformidadId,
                @AnalisisId,
                @Descripcion,
                @Responsable,
                @FechaLimite,
                @Prioridad,
                'PENDIENTE',
                'NO_INTEGRADA',
                @CreadoPor
            );

            SELECT LAST_INSERT_ID();";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                NoConformidadId = noConformidadId,
                request.AnalisisId,
                request.Descripcion,
                request.Responsable,
                request.FechaLimite,
                request.Prioridad,
                request.CreadoPor,
            });
    }

    public async Task<bool> Actualizar(int id, ActualizarAccionCorrectivaRequest request)
    {
        using var connection = _factory.CreateConnection();

        const string sql = @"
            UPDATE nc_acciones_correctivas
            SET
                descripcion = @Descripcion,
                responsable = @Responsable,
                fecha_limite = @FechaLimite,
                prioridad = @Prioridad,
                estado = @Estado,
                actualizado_por = @ActualizadoPor,
                fecha_actualizacion = UTC_TIMESTAMP()
            WHERE id = @Id";

        var filas = await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                request.Descripcion,
                request.Responsable,
                request.FechaLimite,
                request.Prioridad,
                request.Estado,
                request.ActualizadoPor,
            });

        return filas > 0;
    }
}
