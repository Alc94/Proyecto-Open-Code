using ZooLogic.Api.Contracts.Alimentacion;
using ZooLogic.Api.Contracts.Comunes;

namespace ZooLogic.Api.Services;

/// <summary>Servicio de solo lectura de registros de alimentación.</summary>
public interface IRegistroAlimentacionService
{
    /// <summary>Lista registros de alimentación de forma paginada.</summary>
    Task<PaginaResponse<RegistroAlimentacionResponse>> ObtenerAsync(
        int? animalId,
        int? alimentoId,
        DateTimeOffset? desde,
        DateTimeOffset? hasta,
        int pagina,
        int tamano,
        CancellationToken cancellationToken);
}
