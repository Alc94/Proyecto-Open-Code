using Microsoft.EntityFrameworkCore;
using ZooLogic.Api.Contracts.Especies;
using ZooLogic.Api.Data;

namespace ZooLogic.Api.Services;

internal sealed class EspecieService(ZooDbContext db) : IEspecieService
{
    public async Task<IReadOnlyList<EspecieResponse>> ObtenerAsync(
        string? grupo,
        string? tipoDieta,
        string? texto,
        CancellationToken cancellationToken)
    {
        var consulta = db.Especies.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(grupo))
        {
            consulta = consulta.Where(especie => especie.Grupo == grupo);
        }

        if (!string.IsNullOrWhiteSpace(tipoDieta))
        {
            consulta = consulta.Where(especie => especie.TipoDieta == tipoDieta);
        }

        if (!string.IsNullOrWhiteSpace(texto))
        {
            var patron = $"%{EscaparLike(texto.Trim())}%";
            consulta = consulta.Where(especie =>
                EF.Functions.ILike(especie.NombreComun, patron) ||
                EF.Functions.ILike(especie.NombreCientifico, patron));
        }

        return await consulta
            .OrderBy(especie => EF.Functions.Collate(especie.NombreComun, Colaciones.Espanol))
            .ThenBy(especie => especie.Id)
            .Select(especie => new EspecieResponse(
                especie.Id,
                especie.NombreComun,
                especie.NombreCientifico,
                especie.Grupo,
                especie.TipoDieta,
                especie.EstadoConservacion,
                especie.Animales.Count,
                especie.Dietas.Count))
            .ToListAsync(cancellationToken);
    }

    public async Task<EspecieResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken) =>
        await db.Especies
            .AsNoTracking()
            .Where(especie => especie.Id == id)
            .Select(especie => new EspecieResponse(
                especie.Id,
                especie.NombreComun,
                especie.NombreCientifico,
                especie.Grupo,
                especie.TipoDieta,
                especie.EstadoConservacion,
                especie.Animales.Count,
                especie.Dietas.Count))
            .FirstOrDefaultAsync(cancellationToken);

    private static string EscaparLike(string valor) =>
        valor.Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("%", "\\%", StringComparison.Ordinal)
            .Replace("_", "\\_", StringComparison.Ordinal);
}
