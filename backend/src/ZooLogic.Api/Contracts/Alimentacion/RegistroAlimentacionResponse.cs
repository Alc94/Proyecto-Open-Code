namespace ZooLogic.Api.Contracts.Alimentacion;

/// <summary>Registro de alimentación de un animal.</summary>
/// <param name="Id">Identificador del registro.</param>
/// <param name="AnimalId">Identificador del animal alimentado.</param>
/// <param name="AnimalNombre">Nombre del animal alimentado.</param>
/// <param name="AlimentoId">Identificador del alimento suministrado.</param>
/// <param name="AlimentoNombre">Nombre del alimento suministrado.</param>
/// <param name="Fecha">Fecha y hora del registro.</param>
/// <param name="CantidadKg">Cantidad suministrada en kilogramos.</param>
/// <param name="Consumido">Porcentaje consumido, entre 0 y 100.</param>
/// <param name="KcalAportadas">Kilocalorías aportadas por la cantidad suministrada.</param>
/// <param name="CoincideConDietaValidada">Indica si el alimento figura en una dieta validada de su especie.</param>
public sealed record RegistroAlimentacionResponse(
    int Id,
    int AnimalId,
    string AnimalNombre,
    int AlimentoId,
    string AlimentoNombre,
    DateTimeOffset Fecha,
    decimal CantidadKg,
    decimal Consumido,
    decimal KcalAportadas,
    bool CoincideConDietaValidada);
