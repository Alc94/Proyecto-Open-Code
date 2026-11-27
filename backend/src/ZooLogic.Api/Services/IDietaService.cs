using ZooLogic.Api.Contracts.Alimentacion;

namespace ZooLogic.Api.Services;

/// <summary>Servicio de solo lectura de dietas.</summary>
public interface IDietaService
{
    /// <summary>Lista dietas por especie y estado.</summary>
    Task<IReadOnlyList<DietaListResponse>> ObtenerAsync(
        int? especieId, string? estado, CancellationToken cancellationToken);

    /// <summary>Obtiene una dieta con sus líneas.</summary>
    Task<DietaResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken);
}
