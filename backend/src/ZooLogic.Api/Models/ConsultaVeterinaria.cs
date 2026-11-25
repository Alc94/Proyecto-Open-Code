namespace ZooLogic.Api.Models;

public class ConsultaVeterinaria
{
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public DateTimeOffset Fecha { get; set; }
    public string Veterinario { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;
    public string Diagnostico { get; set; } = string.Empty;
    public decimal? PesoKg { get; set; }
    public decimal? Temperatura { get; set; }

    public Animal Animal { get; set; } = null!;
    public ICollection<Tratamiento> Tratamientos { get; set; } = new List<Tratamiento>();
}
