using Microsoft.EntityFrameworkCore;
using ZooLogic.Api.Contracts.Comunes;
using ZooLogic.Api.Contracts.Consultas;
using ZooLogic.Api.Data;
using ZooLogic.Api.Exceptions;
using ZooLogic.Api.Models;

namespace ZooLogic.Api.Services;

/// <summary>Implementacion de <see cref="IConsultaService"/> con EF Core.</summary>
internal sealed class ConsultaService(ZooDbContext db) : IConsultaService
{
    /// <inheritdoc />
    public async Task<PaginaResponse<ConsultaResumenResponse>> ObtenerAsync(
        int? animalId, DateTimeOffset? desde, DateTimeOffset? hasta,
        int pagina, int tamano, CancellationToken cancellationToken)
    {
        var consulta = db.Consultas.AsNoTracking();

        if (animalId is > 0)
            consulta = consulta.Where(c => c.AnimalId == animalId.Value);
        if (desde.HasValue)
            consulta = consulta.Where(c => c.Fecha >= desde.Value);
        if (hasta.HasValue)
            consulta = consulta.Where(c => c.Fecha <= hasta.Value);

        var total = await consulta.CountAsync(cancellationToken);
        var elementos = await consulta
            .OrderByDescending(c => c.Fecha)
            .ThenBy(c => c.Id)
            .Skip((pagina - 1) * tamano)
            .Take(tamano)
            .Select(c => new ConsultaResumenResponse(
                c.Id,
                c.AnimalId,
                c.Animal.Nombre,
                c.Fecha,
                c.Veterinario,
                c.Motivo,
                c.Diagnostico))
            .ToListAsync(cancellationToken);

        return new PaginaResponse<ConsultaResumenResponse>(elementos, total, pagina, tamano);
    }

    /// <inheritdoc />
    public Task<ConsultaDetalleResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken) =>
        db.Consultas
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new ConsultaDetalleResponse(
                c.Id,
                c.AnimalId,
                c.Animal.Nombre,
                c.Fecha,
                c.Veterinario,
                c.Motivo,
                c.Diagnostico,
                c.PesoKg,
                c.Temperatura,
                c.Tratamientos
                    .OrderBy(t => t.FechaInicio)
                    .Select(t => new TratamientoResponse(
                        t.Id,
                        t.ConsultaId,
                        c.AnimalId,
                        c.Animal.Nombre,
                        t.Medicamento,
                        t.Dosis,
                        t.FechaInicio,
                        t.FechaFin,
                        t.Notas,
                        t.FechaFin == null))
                    .ToList()))
            .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<ConsultaDetalleResponse> CrearAsync(
        CrearConsultaRequest request, CancellationToken cancellationToken)
    {
        if (request.Fecha > DateTimeOffset.UtcNow)
            throw new ValidacionException("Fecha", "La fecha de la consulta no puede ser futura.");

        var animal = await db.Animales
            .FirstOrDefaultAsync(a => a.Id == request.AnimalId, cancellationToken);
        if (animal is null)
            throw new ValidacionException("AnimalId", "El animal indicado no existe.");

        var tratamientos = new List<Tratamiento>();
        var numero = 0;
        foreach (var item in request.Tratamientos ?? [])
        {
            if (item.FechaInicio > DateTimeOffset.UtcNow)
                throw new ValidacionException(
                    $"Tratamientos[{numero}].FechaInicio", "La fecha de inicio no puede ser futura.");
            if (item.FechaFin.HasValue && item.FechaFin.Value < item.FechaInicio)
                throw new ValidacionException(
                    $"Tratamientos[{numero}].FechaFin",
                    "La fecha de fin no puede ser anterior a la fecha de inicio.");

            tratamientos.Add(new Tratamiento
            {
                Medicamento = item.Medicamento.Trim(),
                Dosis = item.Dosis.Trim(),
                FechaInicio = item.FechaInicio,
                FechaFin = item.FechaFin,
                Notas = string.IsNullOrWhiteSpace(item.Notas) ? null : item.Notas.Trim()
            });
            numero++;
        }

        var consulta = new ConsultaVeterinaria
        {
            AnimalId = request.AnimalId,
            Fecha = request.Fecha,
            Veterinario = request.Veterinario.Trim(),
            Motivo = request.Motivo.Trim(),
            Diagnostico = request.Diagnostico.Trim(),
            PesoKg = request.PesoKg,
            Temperatura = request.Temperatura,
            Tratamientos = tratamientos
        };

        if (tratamientos.Any(t => t.FechaFin is null))
            animal.Estado = "EnTratamiento";

        db.Consultas.Add(consulta);
        await db.SaveChangesAsync(cancellationToken);

        return await ObtenerPorIdAsync(consulta.Id, cancellationToken)
            ?? throw new InvalidOperationException("No se pudo leer la consulta creada.");
    }
}
