using ZooLogic.Api.Contracts.Alimentacion;

namespace ZooLogic.Api.Services;

/// <summary>Servicio de solo lectura de alimentos.</summary>
public interface IAlimentoService
{
    /// <summary>Lista alimentos por categoría y texto.</summary>
    Task<IReadOnlyList<AlimentoResponse>> ObtenerAsync(
        string? categoria, string? texto, CancellationToken cancellationToken);
}
