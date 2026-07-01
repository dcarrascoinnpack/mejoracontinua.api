namespace MejoraContinua.Api.Models;

public class CrearAccionCorrectivaRequest
{
    public int? AnalisisId { get; set; }

    public string Descripcion { get; set; } = string.Empty;

    public string Responsable { get; set; } = string.Empty;

    public DateTime FechaLimite { get; set; }

    public string? Prioridad { get; set; }

    public string? CreadoPor { get; set; }
}
