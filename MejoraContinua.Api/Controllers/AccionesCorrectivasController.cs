using MejoraContinua.Api.Models;
using MejoraContinua.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace MejoraContinua.Api.Controllers;

[ApiController]
public class AccionesCorrectivasController : ControllerBase
{
    private static readonly string[] PrioridadesValidas = { "ALTA", "MEDIA", "BAJA" };
    private static readonly string[] EstadosValidos = { "PENDIENTE", "EN_PROCESO", "COMPLETADA", "CANCELADA" };

    private readonly AccionCorrectivaRepository _repository;

    public AccionesCorrectivasController(AccionCorrectivaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("api/no-conformidades/{noConformidadId}/acciones")]
    public async Task<IActionResult> GetPorNoConformidad(int noConformidadId)
    {
        if (!await _repository.ExisteNoConformidad(noConformidadId))
            return NotFound(new { mensaje = "No conformidad no encontrada" });

        var acciones = await _repository.ObtenerPorNoConformidadId(noConformidadId);

        return Ok(acciones);
    }

    [HttpPost("api/no-conformidades/{noConformidadId}/acciones")]
    public async Task<IActionResult> Post(int noConformidadId, [FromBody] CrearAccionCorrectivaRequest request)
    {
        if (!await _repository.ExisteNoConformidad(noConformidadId))
            return NotFound(new { mensaje = "No conformidad no encontrada" });

        var error = ValidarCamposObligatorios(request.Descripcion, request.Responsable, request.FechaLimite, request.Prioridad);
        if (error != null)
            return BadRequest(new { mensaje = error });

        try
        {
            var id = await _repository.Crear(noConformidadId, request);
            return Ok(new { id });
        }
        catch (MySqlException)
        {
            return BadRequest(new { mensaje = "No se pudo guardar la acción. Verifique que analisisId (si se envió) exista y pertenezca a esta no conformidad." });
        }
    }

    [HttpPut("api/acciones-correctivas/{accionId}")]
    public async Task<IActionResult> Put(int accionId, [FromBody] ActualizarAccionCorrectivaRequest request)
    {
        var existente = await _repository.ObtenerPorId(accionId);
        if (existente == null)
            return NotFound(new { mensaje = "Acción correctiva no encontrada" });

        var error = ValidarCamposObligatorios(request.Descripcion, request.Responsable, request.FechaLimite, request.Prioridad);
        if (error != null)
            return BadRequest(new { mensaje = error });

        if (!EstadosValidos.Contains(request.Estado))
            return BadRequest(new { mensaje = $"estado inválido. Valores permitidos: {string.Join(", ", EstadosValidos)}" });

        await _repository.Actualizar(accionId, request);

        return Ok(new { mensaje = "Acción correctiva actualizada correctamente" });
    }

    private static string? ValidarCamposObligatorios(string descripcion, string responsable, DateTime fechaLimite, string? prioridad)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
            return "descripcion es obligatoria";

        if (string.IsNullOrWhiteSpace(responsable))
            return "responsable es obligatorio";

        if (fechaLimite == default)
            return "fechaLimite es obligatoria";

        if (prioridad != null && !PrioridadesValidas.Contains(prioridad))
            return $"prioridad inválida. Valores permitidos: {string.Join(", ", PrioridadesValidas)}";

        return null;
    }
}
