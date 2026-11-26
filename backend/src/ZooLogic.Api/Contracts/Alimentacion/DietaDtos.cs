namespace ZooLogic.Api.Contracts.Alimentacion;

/// <summary>Dieta de una especie con sus líneas de alimento.</summary>
/// <param name="Id">Identificador de la dieta.</param>
/// <param name="EspecieId">Identificador de la especie.</param>
/// <param name="EspecieNombreComun">Nombre común de la especie.</param>
/// <param name="Nombre">Nombre de la dieta.</param>
/// <param name="Estado">Estado: Propuesta, Validada o Descartada.</param>
/// <param name="ValidadaPor">Veterinario que validó la dieta, si existe.</param>
/// <param name="Origen">Origen: Manual o IA simulada.</param>
/// <param name="NumeroLineas">Número de líneas de alimento.</param>
/// <param name="KcalTotalesDia">Kilocalorías totales estimadas por día.</param>
/// <param name="PuedeAplicarse">Indica si la dieta está validada y puede aplicarse.</param>
/// <param name="Lineas">Líneas de alimento de la dieta.</param>
public sealed record DietaResponse(
    int Id,
    int EspecieId,
    string EspecieNombreComun,
    string Nombre,
    string Estado,
    string? ValidadaPor,
    string Origen,
    int NumeroLineas,
    decimal KcalTotalesDia,
    bool PuedeAplicarse,
    IReadOnlyList<DietaLineaResponse> Lineas);

/// <summary>Línea de alimento de una dieta.</summary>
/// <param name="Id">Identificador de la línea.</param>
/// <param name="AlimentoId">Identificador del alimento.</param>
/// <param name="AlimentoNombre">Nombre del alimento.</param>
/// <param name="Categoria">Categoría del alimento.</param>
/// <param name="CantidadKgDia">Cantidad diaria en kilogramos.</param>
/// <param name="Frecuencia">Frecuencia de suministro.</param>
/// <param name="KcalDia">Kilocalorías diarias aportadas por la línea.</param>
public sealed record DietaLineaResponse(
    int Id,
    int AlimentoId,
    string AlimentoNombre,
    string Categoria,
    decimal CantidadKgDia,
    string Frecuencia,
    decimal KcalDia);
