using ZooLogic.Api.Contracts.Panel;

namespace ZooLogic.Api.Services;

/// <summary>Servicio de solo lectura del resumen del panel.</summary>
public interface IPanelService
{
    /// <summary>Obtiene los totales que se muestran en el panel principal.</summary>
    Task<ResumenPanelResponse> ObtenerResumenAsync(CancellationToken cancellationToken);
}
