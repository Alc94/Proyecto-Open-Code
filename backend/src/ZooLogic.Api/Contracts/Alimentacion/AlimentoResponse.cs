namespace ZooLogic.Api.Contracts.Alimentacion;

/// <summary>Alimento disponible en el zoo.</summary>
/// <param name="Id">Identificador del alimento.</param>
/// <param name="Nombre">Nombre del alimento.</param>
/// <param name="Categoria">Categoría del alimento.</param>
/// <param name="KcalPorKg">Kilocalorías por kilogramo.</param>
public sealed record AlimentoResponse(
    int Id,
    string Nombre,
    string Categoria,
    decimal KcalPorKg);
