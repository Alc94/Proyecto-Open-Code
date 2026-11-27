using System.ComponentModel.DataAnnotations;
using ZooLogic.Api.Validation;

namespace ZooLogic.Api.Contracts.Alertas;

/// <summary>Datos para cambiar el estado de una alerta de salud.</summary>
public sealed record CambiarEstadoAlertaRequest
{
    /// <summary>Nuevo estado de la alerta.</summary>
    [Required, ValorDeDominio(nameof(ValoresDominio.EstadosAlerta))]
    public required string Estado { get; init; }
}
