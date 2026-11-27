using Microsoft.EntityFrameworkCore;
using ZooLogic.Api.Contracts.Alimentacion;
using ZooLogic.Api.Contracts.Comunes;
using ZooLogic.Api.Data;

namespace ZooLogic.Api.Services;

/// <summary>Implementacion de <see cref="IRegistroAlimentacionService"/> con EF Core.</summary>
internal sealed class RegistroAlimentacionService(ZooDbContext db) : IRegistroAlimentacionService
{
    /// <inheritdoc />
    public async Task<PaginaResponse<RegistroAlimentacionResponse>> ObtenerAsync(
        int? animalId,
        int? alimentoId,
        DateTimeOffset? desde,
        DateTimeOffset? hasta,
        int pagina,
        int tamano,
        CancellationToken cancellationToken)
    {
        var consulta = db.RegistrosAlimentacion.AsNoTracking();

        if (animalId is > 0)
        {
            consulta = consulta.Where(r => r.AnimalId == animalId.Value);
        }

        if (alimentoId is > 0)
        {
            consulta = consulta.Where(r => r.AlimentoId == alimentoId.Value);
        }

        if (desde is not null)
        {
            consulta = consulta.Where(r => r.Fecha >= desde.Value);
        }

        if (hasta is not null)
        {
            consulta = consulta.Where(r => r.Fecha <= hasta.Value);
        }

        var total = await consulta.CountAsync(cancellationToken);

        var elementos = await consulta
            .OrderByDescending(r => r.Fecha)
            .ThenByDescending(r => r.Id)
            .Skip((pagina - 1) * tamano)
            .Take(tamano)
            .Select(r => new RegistroAlimentacionResponse(
                r.Id,
                r.AnimalId,
                r.Animal.Nombre,
                r.AlimentoId,
                r.Alimento.Nombre,
                r.Fecha,
                r.CantidadKg,
                r.Consumido,
                r.CantidadKg * r.Alimento.KcalPorKg,
                db.Dietas.Any(d => d.EspecieId == r.Animal.EspecieId &&
                                   d.Estado == "Validada" &&
                                   d.Lineas.Any(l => l.AlimentoId == r.AlimentoId))))
            .ToListAsync(cancellationToken);

        return new PaginaResponse<RegistroAlimentacionResponse>(elementos, total, pagina, tamano);
    }
}
