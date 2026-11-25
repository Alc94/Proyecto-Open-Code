namespace ZooLogic.Api.Models;

public class AlertaSalud
{
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public DateTimeOffset Fecha { get; set; }
    public string Gravedad { get; set; } = string.Empty; // Baja, Media, Alta, Critica
    public string Tipo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public string Estado { get; set; } = "Abierta"; // Abierta, Revisada, Cerrada
    public string Origen { get; set; } = "IA simulada";

    public Animal Animal { get; set; } = null!;
}
