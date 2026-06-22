namespace MejoraContinua.Api.Models;

public class NoConformidad
{
    public int Id { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Tipo { get; set; } = string.Empty;

    public string Origen { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public string Severidad { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public string? Proceso { get; set; }

    public string? Norma { get; set; }

    public DateTime FechaCreacion { get; set; }
}
