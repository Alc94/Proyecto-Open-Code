using ZooLogic.Api.Contracts.Especies;

namespace ZooLogic.Api.Services;

/// <summary>Consultas de solo lectura del catálogo de especies.</summary>
public interface IEspecieService
{
    /// <summary>Devuelve las especies que cumplen los filtros indicados.</summary>
    Task<IReadOnlyList<EspecieResponse>> ObtenerAsync(
        string? grupo,
        string? tipoDieta,
        string? texto,
        CancellationToken cancellationToken);

    /// <summary>Devuelve una especie por su identificador o <c>null</c> si no existe.</summary>
    Task<EspecieResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken);
}
