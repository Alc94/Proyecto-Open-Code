using Microsoft.EntityFrameworkCore;
using ZooLogic.Api.Contracts.Alimentacion;
using ZooLogic.Api.Data;

namespace ZooLogic.Api.Services;

/// <summary>Implementacion de <see cref="IDietaService"/> con EF Core.</summary>
internal sealed class DietaService(ZooDbContext db) : IDietaService
{
    /// <inheritdoc />
    public async Task<IReadOnlyList<DietaListResponse>> ObtenerAsync(
        int? especieId, string? estado, CancellationToken cancellationToken)
    {
        var consulta = db.Dietas.AsNoTracking();

        if (especieId is > 0)
        {
            consulta = consulta.Where(d => d.EspecieId == especieId.Value);
        }

        if (!string.IsNullOrWhiteSpace(estado))
        {
            consulta = consulta.Where(d => d.Estado == estado);
        }

        return await consulta
            .OrderBy(d => d.Id)
            .Select(d => new DietaListResponse(
                d.Id,
                d.EspecieId,
                d.Especie.NombreComun,
                d.Nombre,
                d.Estado,
                d.ValidadaPor,
                d.Origen,
                d.Lineas.Count(),
                d.Lineas.Sum(l => l.CantidadKgDia * l.Alimento.KcalPorKg),
                d.Estado == "Validada"))
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task<DietaResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken) =>
        db.Dietas
            .AsNoTracking()
            .Where(d => d.Id == id)
            .Select(d => new DietaResponse(
                d.Id,
                d.EspecieId,
                d.Especie.NombreComun,
                d.Nombre,
                d.Estado,
                d.ValidadaPor,
                d.Origen,
                d.Lineas.Count(),
                d.Lineas.Sum(l => l.CantidadKgDia * l.Alimento.KcalPorKg),
                d.Estado == "Validada",
                d.Lineas
                    .OrderBy(l => l.Id)
                    .Select(l => new DietaLineaResponse(
                        l.Id,
                        l.AlimentoId,
                        l.Alimento.Nombre,
                        l.Alimento.Categoria,
                        l.CantidadKgDia,
                        l.Frecuencia,
                        l.CantidadKgDia * l.Alimento.KcalPorKg))
                    .ToList()))
            .FirstOrDefaultAsync(cancellationToken);
}
