using ZooLogic.Api.Contracts.Consultas;

namespace ZooLogic.Api.Services;

/// <summary>Servicio de tratamientos veterinarios.</summary>
public interface ITratamientoService
{
    /// <summary>Lista tratamientos por animal y estado activo.</summary>
    Task<IReadOnlyList<TratamientoResponse>> ObtenerAsync(
        int? animalId, bool? activo, CancellationToken cancellationToken);

    /// <summary>Obtiene un tratamiento por su identificador.</summary>
    Task<TratamientoResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>Anade un tratamiento a una consulta existente.</summary>
    Task<TratamientoResponse> CrearAsync(
        int consultaId, CrearTratamientoRequest request, CancellationToken cancellationToken);

    /// <summary>Cierra un tratamiento y actualiza el estado del animal si procede.</summary>
    Task<TratamientoResponse> CerrarAsync(
        int id, CerrarTratamientoRequest request, CancellationToken cancellationToken);
}
