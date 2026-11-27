namespace ZooLogic.Api.Contracts.Panel;

/// <summary>Totales que se muestran en el panel principal.</summary>
/// <param name="TotalAnimales">Número total de animales.</param>
/// <param name="AnimalesEnTratamiento">Animales en estado EnTratamiento.</param>
/// <param name="AlertasAbiertas">Alertas en estado Abierta.</param>
/// <param name="AlertasGravesAbiertas">Alertas abiertas de gravedad Alta o Critica.</param>
/// <param name="DietasEnPropuesta">Dietas en estado Propuesta.</param>
public sealed record ResumenPanelResponse(
    int TotalAnimales,
    int AnimalesEnTratamiento,
    int AlertasAbiertas,
    int AlertasGravesAbiertas,
    int DietasEnPropuesta);
