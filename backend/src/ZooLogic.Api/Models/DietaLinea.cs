namespace ZooLogic.Api.Models;

public class DietaLinea
{
    public int Id { get; set; }
    public int DietaId { get; set; }
    public int AlimentoId { get; set; }
    public decimal CantidadKgDia { get; set; }
    public string Frecuencia { get; set; } = string.Empty;

    public Dieta Dieta { get; set; } = null!;
    public Alimento Alimento { get; set; } = null!;
}
