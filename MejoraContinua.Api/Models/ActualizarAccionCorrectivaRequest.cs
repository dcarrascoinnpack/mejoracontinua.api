namespace MejoraContinua.Api.Models;

public class ActualizarAccionCorrectivaRequest
{
    public string Descripcion { get; set; } = string.Empty;

    public string Responsable { get; set; } = string.Empty;

    public DateTime FechaLimite { get; set; }

    public string? Prioridad { get; set; }

    public string Estado { get; set; } = string.Empty;

    public string? ActualizadoPor { get; set; }
}
