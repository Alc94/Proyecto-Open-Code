namespace ZooLogic.Api.Contracts.Alimentacion;

/// <summary>Resumen de una dieta para listados.</summary>
/// <param name="Id">Identificador de la dieta.</param>
/// <param name="EspecieId">Identificador de la especie.</param>
/// <param name="EspecieNombreComun">Nombre común de la especie.</param>
/// <param name="Nombre">Nombre de la dieta.</param>
/// <param name="Estado">Estado: Propuesta, Validada o Descartada.</param>
/// <param name="ValidadaPor">Veterinario que validó la dieta, si existe.</param>
/// <param name="Origen">Origen declarado de la dieta.</param>
/// <param name="NumeroLineas">Número de líneas de alimento.</param>
/// <param name="KcalTotalesDia">Kilocalorías totales estimadas por día.</param>
/// <param name="PuedeAplicarse">Indica si la dieta está validada y puede aplicarse.</param>
public sealed record DietaListResponse(
    int Id,
    int EspecieId,
    string EspecieNombreComun,
    string Nombre,
    string Estado,
    string? ValidadaPor,
    string Origen,
    int NumeroLineas,
    decimal KcalTotalesDia,
    bool PuedeAplicarse);
