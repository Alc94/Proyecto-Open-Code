namespace ZooLogic.Api.Models;

public class RegistroAlimentacion
{
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public DateTimeOffset Fecha { get; set; }
    public int AlimentoId { get; set; }
    public decimal CantidadKg { get; set; }
    public decimal Consumido { get; set; } // porcentaje entre 0 y 100

    public Animal Animal { get; set; } = null!;
    public Alimento Alimento { get; set; } = null!;
}
