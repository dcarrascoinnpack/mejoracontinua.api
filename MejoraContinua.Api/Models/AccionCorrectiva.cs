namespace MejoraContinua.Api.Models;

public class AccionCorrectiva
{
    public int Id { get; set; }

    public int NoConformidadId { get; set; }

    public int? AnalisisId { get; set; }

    public string Descripcion { get; set; } = string.Empty;

    public string Responsable { get; set; } = string.Empty;

    public DateTime FechaLimite { get; set; }

    public string? Prioridad { get; set; }

    public string Estado { get; set; } = string.Empty;

    public string IntegracionTareasEstado { get; set; } = string.Empty;

    public string? TareaId { get; set; }

    public string? CreadoPor { get; set; }

    public DateTime FechaCreacion { get; set; }

    public string? ActualizadoPor { get; set; }

    public DateTime? FechaActualizacion { get; set; }
}
