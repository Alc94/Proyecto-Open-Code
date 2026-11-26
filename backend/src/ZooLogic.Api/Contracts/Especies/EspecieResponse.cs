namespace ZooLogic.Api.Contracts.Especies;

/// <summary>Representa una especie del zoológico con sus conteos asociados.</summary>
/// <param name="Id">Identificador de la especie.</param>
/// <param name="NombreComun">Nombre común de la especie.</param>
/// <param name="NombreCientifico">Nombre científico de la especie.</param>
/// <param name="Grupo">Grupo zoológico: Mamifero, Ave, Reptil, Anfibio o Pez.</param>
/// <param name="TipoDieta">Tipo de dieta: Carnivoro, Herbivoro u Omnivoro.</param>
/// <param name="EstadoConservacion">Estado de conservación según la ficha de la especie.</param>
/// <param name="NumeroAnimales">Cantidad de animales registrados de esta especie.</param>
/// <param name="NumeroDietas">Cantidad de dietas asociadas a esta especie.</param>
public sealed record EspecieResponse(
    int Id,
    string NombreComun,
    string NombreCientifico,
    string Grupo,
    string TipoDieta,
    string EstadoConservacion,
    int NumeroAnimales,
    int NumeroDietas);
