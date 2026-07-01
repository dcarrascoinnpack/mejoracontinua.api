using MejoraContinua.Api.Models;
using MejoraContinua.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace MejoraContinua.Api.Controllers;

[ApiController]
[Route("api/no-conformidades/{noConformidadId}/analisis")]
public class AnalisisNoConformidadController : ControllerBase
{
    private static readonly string[] MetodologiasValidas = { "CINCO_PORQUES", "ISHIKAWA", "MIXTA" };

    private readonly AnalisisCausaRaizRepository _repository;

    public AnalisisNoConformidadController(AnalisisCausaRaizRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int noConformidadId)
    {
        if (!await _repository.ExisteNoConformidad(noConformidadId))
            return NotFound(new { mensaje = "No conformidad no encontrada" });

        var analisis = await _repository.ObtenerPorNoConformidadId(noConformidadId);

        if (analisis == null)
            return NotFound(new { mensaje = "Esta no conformidad aún no tiene un análisis registrado" });

        return Ok(analisis);
    }

    [HttpPost]
    public async Task<IActionResult> Post(int noConformidadId, [FromBody] CrearAnalisisRequest request)
    {
        if (!await _repository.ExisteNoConformidad(noConformidadId))
            return NotFound(new { mensaje = "No conformidad no encontrada" });

        var error = ValidarMetodologiaYProblema(request);
        if (error != null)
            return BadRequest(new { mensaje = error });

        var existente = await _repository.ObtenerPorNoConformidadId(noConformidadId);
        if (existente != null)
            return Conflict(new { mensaje = "Esta no conformidad ya tiene un análisis. Use PUT para actualizarlo." });

        try
        {
            var id = await _repository.Crear(noConformidadId, request);
            return Ok(new { id });
        }
        catch (MySqlException)
        {
            return BadRequest(new { mensaje = "No se pudo guardar el análisis. Verifique los datos enviados." });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Put(int noConformidadId, [FromBody] CrearAnalisisRequest request)
    {
        if (!await _repository.ExisteNoConformidad(noConformidadId))
            return NotFound(new { mensaje = "No conformidad no encontrada" });

        var error = ValidarMetodologiaYProblema(request);
        if (error != null)
            return BadRequest(new { mensaje = error });

        if (string.IsNullOrWhiteSpace(request.CausaRaiz))
            return BadRequest(new { mensaje = "causaRaiz es obligatoria para guardar el análisis final" });

        var actualizado = await _repository.Actualizar(noConformidadId, request);

        if (!actualizado)
            return NotFound(new { mensaje = "Esta no conformidad aún no tiene un análisis. Use POST para crearlo primero." });

        return Ok(new { mensaje = "Análisis actualizado correctamente" });
    }

    private static string? ValidarMetodologiaYProblema(CrearAnalisisRequest request)
    {
        if (!MetodologiasValidas.Contains(request.Metodologia))
            return $"metodologia inválida. Valores permitidos: {string.Join(", ", MetodologiasValidas)}";

        if (string.IsNullOrWhiteSpace(request.ProblemaDetectado))
            return "problemaDetectado es obligatorio";

        return null;
    }
}
