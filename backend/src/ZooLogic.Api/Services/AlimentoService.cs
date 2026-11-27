using Microsoft.EntityFrameworkCore;
using ZooLogic.Api.Contracts.Alimentacion;
using ZooLogic.Api.Data;

namespace ZooLogic.Api.Services;

/// <summary>Implementacion de <see cref="IAlimentoService"/> con EF Core.</summary>
internal sealed class AlimentoService(ZooDbContext db) : IAlimentoService
{
    /// <inheritdoc />
    public async Task<IReadOnlyList<AlimentoResponse>> ObtenerAsync(
        string? categoria, string? texto, CancellationToken cancellationToken)
    {
        var consulta = db.Alimentos.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(categoria))
        {
            consulta = consulta.Where(a => a.Categoria == categoria.Trim());
        }

        if (!string.IsNullOrWhiteSpace(texto))
        {
            var patron = $"%{EscaparLike(texto.Trim())}%";
            consulta = consulta.Where(a => EF.Functions.ILike(a.Nombre, patron));
        }

        return await consulta
            .OrderBy(a => EF.Functions.Collate(a.Nombre, Colaciones.Espanol))
            .ThenBy(a => a.Id)
            .Select(a => new AlimentoResponse(a.Id, a.Nombre, a.Categoria, a.KcalPorKg))
            .ToListAsync(cancellationToken);
    }

    private static string EscaparLike(string texto) =>
        texto.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_");
}
