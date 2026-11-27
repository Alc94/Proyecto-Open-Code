using ZooLogic.Api.Contracts.Comunes;
using ZooLogic.Api.Contracts.Consultas;

namespace ZooLogic.Api.Services;

/// <summary>Servicio de consultas veterinarias.</summary>
public interface IConsultaService
{
    /// <summary>Lista consultas de forma paginada.</summary>
    Task<PaginaResponse<ConsultaResumenResponse>> ObtenerAsync(
        int? animalId, DateTimeOffset? desde, DateTimeOffset? hasta,
        int pagina, int tamano, CancellationToken cancellationToken);

    /// <summary>Obtiene una consulta con sus tratamientos.</summary>
    Task<ConsultaDetalleResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>Crea una consulta con sus tratamientos anidados.</summary>
    Task<ConsultaDetalleResponse> CrearAsync(CrearConsultaRequest request, CancellationToken cancellationToken);
}
