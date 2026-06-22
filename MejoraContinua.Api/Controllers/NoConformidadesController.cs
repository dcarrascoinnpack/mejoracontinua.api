using MejoraContinua.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using MejoraContinua.Api.Models;

namespace MejoraContinua.Api.Controllers;

[ApiController]
[Route("api/no-conformidades")]
public class NoConformidadesController : ControllerBase
{
    private readonly NoConformidadRepository _repository;

    public NoConformidadesController(
        NoConformidadRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var datos =
            await _repository.ObtenerTodas();

        return Ok(datos);
    }
    [HttpPost]
public async Task<IActionResult> Post(
    [FromBody] CreateNoConformidadRequest request)
{
    var id = await _repository.Crear(request);

    return Ok(new
    {
        id
    });
}
[HttpGet("{id}")]
public async Task<IActionResult> GetById(int id)
{
    var nc = await _repository.ObtenerPorId(id);

    if (nc == null)
        return NotFound();

    return Ok(nc);
}
[HttpPut("{id}")]
public async Task<IActionResult> Put(
    int id,
    [FromBody] CreateNoConformidadRequest request)
{
    var actualizado =
        await _repository.Actualizar(id, request);

    if (!actualizado)
        return NotFound();

    return Ok(new
    {
        mensaje = "Actualizado correctamente"
    });
}
}
