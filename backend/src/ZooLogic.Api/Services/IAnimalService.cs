using ZooLogic.Api.Contracts.Alimentacion;
using ZooLogic.Api.Contracts.Animales;
using ZooLogic.Api.Contracts.Comunes;

namespace ZooLogic.Api.Services;

/// <summary>Consultas y operaciones sobre animales.</summary>
public interface IAnimalService
{
    /// <summary>Lista animales con filtros y paginación.</summary>
    Task<PaginaResponse<AnimalListResponse>> ObtenerAsync(
        string? estado,
        int? especieId,
        string? recinto,
        string? texto,
        int pagina,
        int tamano,
        CancellationToken cancellationToken);

    /// <summary>Obtiene la ficha de un animal por su identificador.</summary>
    Task<AnimalDetalleResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>Obtiene el historial clínico y de alimentación de un animal.</summary>
    Task<HistorialAnimalResponse?> ObtenerHistorialAsync(int id, CancellationToken cancellationToken);

    /// <summary>Obtiene la dieta validada de la especie del animal.</summary>
    Task<DietaResponse?> ObtenerDietaAsync(int id, CancellationToken cancellationToken);

    /// <summary>Crea un animal y devuelve su ficha.</summary>
    Task<AnimalDetalleResponse> CrearAsync(
        CrearAnimalRequest request,
        CancellationToken cancellationToken);

    /// <summary>Actualiza un animal y devuelve su ficha; null si no existe.</summary>
    Task<AnimalDetalleResponse?> ActualizarAsync(
        int id,
        ActualizarAnimalRequest request,
        CancellationToken cancellationToken);
}
