using Microsoft.EntityFrameworkCore;
using ZooLogic.Api.Contracts.Panel;
using ZooLogic.Api.Data;

namespace ZooLogic.Api.Services;

/// <summary>Implementacion de <see cref="IPanelService"/> con EF Core.</summary>
internal sealed class PanelService(ZooDbContext db) : IPanelService
{
    /// <inheritdoc />
    public async Task<ResumenPanelResponse> ObtenerResumenAsync(CancellationToken cancellationToken)
    {
        var resumen = await db.Animales
            .AsNoTracking()
            .Take(1)
            .Select(_ => new ResumenPanelResponse(
                db.Animales.Count(),
                db.Animales.Count(a => a.Estado == "EnTratamiento"),
                db.AlertasSalud.Count(a => a.Estado == "Abierta"),
                db.AlertasSalud.Count(a => a.Estado == "Abierta" &&
                                            (a.Gravedad == "Alta" || a.Gravedad == "Critica")),
                db.Dietas.Count(d => d.Estado == "Propuesta")))
            .FirstOrDefaultAsync(cancellationToken);

        return resumen ?? new ResumenPanelResponse(0, 0, 0, 0, 0);
    }
}
