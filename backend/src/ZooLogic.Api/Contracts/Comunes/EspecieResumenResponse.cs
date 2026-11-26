namespace ZooLogic.Api.Contracts.Comunes;

/// <summary>Resumen de una especie para incrustar en otros recursos.</summary>
/// <param name="Id">Identificador de la especie.</param>
/// <param name="NombreComun">Nombre común de la especie.</param>
/// <param name="NombreCientifico">Nombre científico de la especie.</param>
/// <param name="Grupo">Grupo zoológico al que pertenece.</param>
/// <param name="TipoDieta">Tipo de dieta de la especie.</param>
public sealed record EspecieResumenResponse(
    int Id,
    string NombreComun,
    string NombreCientifico,
    string Grupo,
    string TipoDieta);
