using ZooLogic.Api.Contracts.Alertas;

namespace ZooLogic.Api.Services;

/// <summary>Servicio de alertas de salud del zoologico.</summary>
public interface IAlertaService
{
    /// <summary>Lista alertas filtradas, ordenadas por gravedad y fecha.</summary>
    Task<IReadOnlyList<AlertaResponse>> ObtenerAsync(
        string? estado, string? gravedad, int? animalId, CancellationToken cancellationToken);

    /// <summary>Obtiene una alerta por su identificador.</summary>
    Task<AlertaResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>Cambia el estado de una alerta siguiendo la secuencia permitida.</summary>
    Task<AlertaResponse> CambiarEstadoAsync(
        int id, CambiarEstadoAlertaRequest request, CancellationToken cancellationToken);
}
