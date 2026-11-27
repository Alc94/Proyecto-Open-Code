using Microsoft.AspNetCore.Http.HttpResults;
using ZooLogic.Api.Contracts.Panel;
using ZooLogic.Api.Services;

namespace ZooLogic.Api.Endpoints;

/// <summary>Endpoints de solo lectura del panel principal.</summary>
public static class PanelEndpoints
{
    /// <summary>Registra los endpoints del panel.</summary>
    public static WebApplication MapPanel(this WebApplication app)
    {
        app.MapGet("/api/panel/resumen", async Task<Ok<ResumenPanelResponse>> (
                IPanelService servicio,
                CancellationToken cancellationToken) =>
            {
                var resumen = await servicio.ObtenerResumenAsync(cancellationToken);
                return TypedResults.Ok(resumen);
            })
            .WithName("ObtenerResumenPanel")
            .WithSummary("Obtiene el resumen del panel principal.")
            .WithDescription("Incluye totales de animales, tratamientos, alertas abiertas, alertas graves y dietas propuestas.")
            .Produces<ResumenPanelResponse>(StatusCodes.Status200OK)
            .WithTags("Panel");

        return app;
    }
}
