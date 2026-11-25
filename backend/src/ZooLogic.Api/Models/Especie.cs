namespace ZooLogic.Api.Models;

public class Especie
{
    public int Id { get; set; }
    public string NombreComun { get; set; } = string.Empty;
    public string NombreCientifico { get; set; } = string.Empty;
    public string Grupo { get; set; } = string.Empty; // Mamifero, Ave, Reptil, Anfibio, Pez
    public string TipoDieta { get; set; } = string.Empty; // Carnivoro, Herbivoro, Omnivoro
    public string EstadoConservacion { get; set; } = string.Empty;

    public ICollection<Animal> Animales { get; set; } = new List<Animal>();
    public ICollection<Dieta> Dietas { get; set; } = new List<Dieta>();
}
