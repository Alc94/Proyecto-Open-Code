namespace ZooLogic.Api.Models;

public class Animal
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int EspecieId { get; set; }
    public string Sexo { get; set; } = string.Empty; // Macho, Hembra
    public DateTimeOffset FechaNacimiento { get; set; }
    public decimal PesoKg { get; set; }
    public string Recinto { get; set; } = string.Empty;
    public string Estado { get; set; } = "Activo"; // Activo, EnObservacion, EnTratamiento, Cuarentena
    public string? FotoUrl { get; set; }

    public Especie Especie { get; set; } = null!;
    public ICollection<ConsultaVeterinaria> Consultas { get; set; } = new List<ConsultaVeterinaria>();
    public ICollection<AlertaSalud> Alertas { get; set; } = new List<AlertaSalud>();
    public ICollection<RegistroAlimentacion> RegistrosAlimentacion { get; set; } = new List<RegistroAlimentacion>();
}
