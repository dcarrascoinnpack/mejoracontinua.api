namespace MejoraContinua.Api.Models;

public class AnalisisCausaRaiz
{
    public int Id { get; set; }

    public int NoConformidadId { get; set; }

    public string Metodologia { get; set; } = string.Empty;

    public string ProblemaDetectado { get; set; } = string.Empty;

    public string? Porque1 { get; set; }

    public string? Porque2 { get; set; }

    public string? Porque3 { get; set; }

    public string? Porque4 { get; set; }

    public string? Porque5 { get; set; }

    public string? CausaRaiz { get; set; }

    public string? Conclusion { get; set; }

    public string? CreadoPor { get; set; }

    public DateTime FechaCreacion { get; set; }

    public string? ActualizadoPor { get; set; }

    public DateTime? FechaActualizacion { get; set; }
}
