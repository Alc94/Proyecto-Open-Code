namespace ZooLogic.Api.Models;

public class Alimento
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal KcalPorKg { get; set; }

    public ICollection<DietaLinea> DietaLineas { get; set; } = new List<DietaLinea>();
    public ICollection<RegistroAlimentacion> RegistrosAlimentacion { get; set; } = new List<RegistroAlimentacion>();
}
