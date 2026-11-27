using Microsoft.EntityFrameworkCore;
using ZooLogic.Api.Contracts.Alertas;
using ZooLogic.Api.Data;
using ZooLogic.Api.Exceptions;

namespace ZooLogic.Api.Services;

/// <summary>Implementacion de <see cref="IAlertaService"/> con EF Core.</summary>
internal sealed class AlertaService(ZooDbContext db) : IAlertaService
{
    /// <inheritdoc />
    public async Task<IReadOnlyList<AlertaResponse>> ObtenerAsync(
        string? estado, string? gravedad, int? animalId, CancellationToken cancellationToken)
    {
        var consulta = db.AlertasSalud.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(estado))
            consulta = consulta.Where(a => a.Estado == estado);
        if (!string.IsNullOrWhiteSpace(gravedad))
            consulta = consulta.Where(a => a.Gravedad == gravedad);
        if (animalId is > 0)
            consulta = consulta.Where(a => a.AnimalId == animalId.Value);

        return await consulta
            .OrderByDescending(a => a.Gravedad == "Critica" ? 4
                : a.Gravedad == "Alta" ? 3
                : a.Gravedad == "Media" ? 2
                : 1)
            .ThenByDescending(a => a.Fecha)
            .ThenBy(a => a.Id)
            .Select(a => new AlertaResponse(
                a.Id,
                a.AnimalId,
                a.Animal.Nombre,
                a.Animal.Especie.NombreComun,
                a.Fecha,
                a.Gravedad,
                a.Tipo,
                a.Mensaje,
                a.Estado,
                a.Origen,
                a.Gravedad == "Alta" || a.Gravedad == "Critica"))
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task<AlertaResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken) =>
        db.AlertasSalud
            .AsNoTracking()
            .Where(a => a.Id == id)
            .Select(a => new AlertaResponse(
                a.Id,
                a.AnimalId,
                a.Animal.Nombre,
                a.Animal.Especie.NombreComun,
                a.Fecha,
                a.Gravedad,
                a.Tipo,
                a.Mensaje,
                a.Estado,
                a.Origen,
                a.Gravedad == "Alta" || a.Gravedad == "Critica"))
            .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<AlertaResponse> CambiarEstadoAsync(
        int id, CambiarEstadoAlertaRequest request, CancellationToken cancellationToken)
    {
        var alerta = await db.AlertasSalud
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
            ?? throw new RecursoNoEncontradoException("No se encontro la alerta indicada.");

        var transicionValida = (alerta.Estado, request.Estado) is
            ("Abierta", "Revisada") or ("Revisada", "Cerrada");

        if (!transicionValida)
            throw new ReglaNegocioException(
                $"No se puede cambiar una alerta de {alerta.Estado} a {request.Estado}.");

        alerta.Estado = request.Estado;
        await db.SaveChangesAsync(cancellationToken);

        return await ObtenerPorIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException("No se pudo leer la alerta actualizada.");
    }
}
