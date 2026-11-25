namespace ZooLogic.Api.Models;

public class Tratamiento
{
    public int Id { get; set; }
    public int ConsultaId { get; set; }
    public string Medicamento { get; set; } = string.Empty;
    public string Dosis { get; set; } = string.Empty;
    public DateTimeOffset FechaInicio { get; set; }
    public DateTimeOffset? FechaFin { get; set; }
    public string? Notas { get; set; }

    public ConsultaVeterinaria Consulta { get; set; } = null!;
}
