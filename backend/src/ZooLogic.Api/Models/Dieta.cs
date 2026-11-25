namespace ZooLogic.Api.Models;

public class Dieta
{
    public int Id { get; set; }
    public int EspecieId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Estado { get; set; } = "Propuesta"; // Propuesta, Validada, Descartada
    public string? ValidadaPor { get; set; }
    public string Origen { get; set; } = "Manual"; // Manual, IA simulada

    public Especie Especie { get; set; } = null!;
    public ICollection<DietaLinea> Lineas { get; set; } = new List<DietaLinea>();
}
