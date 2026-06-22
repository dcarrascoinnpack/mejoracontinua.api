namespace MejoraContinua.Api.Models;

public class CreateNoConformidadRequest
{
    public string Tipo { get; set; } = string.Empty;

    public string Origen { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public string Severidad { get; set; } = string.Empty;

    public string? Proceso { get; set; }

    public string? Norma { get; set; }

    public string? ReportadoPor { get; set; }

    public string? Responsable { get; set; }

    public DateTime FechaDeteccion { get; set; }
}
