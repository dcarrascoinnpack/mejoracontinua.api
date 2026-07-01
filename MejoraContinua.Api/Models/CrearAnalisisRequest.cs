namespace MejoraContinua.Api.Models;

public class CrearAnalisisRequest
{
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

    public string? ActualizadoPor { get; set; }
}
