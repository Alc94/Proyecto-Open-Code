using Microsoft.EntityFrameworkCore;
using ZooLogic.Api.Contracts.Consultas;
using ZooLogic.Api.Data;
using ZooLogic.Api.Exceptions;
using ZooLogic.Api.Models;

namespace ZooLogic.Api.Services;

/// <summary>Implementacion de <see cref="ITratamientoService"/> con EF Core.</summary>
internal sealed class TratamientoService(ZooDbContext db) : ITratamientoService
{
    /// <inheritdoc />
    public async Task<IReadOnlyList<TratamientoResponse>> ObtenerAsync(
        int? animalId, bool? activo, CancellationToken cancellationToken)
    {
        var consulta = db.Tratamientos.AsNoTracking();

        if (animalId is > 0)
            consulta = consulta.Where(t => t.Consulta.AnimalId == animalId.Value);
        if (activo.HasValue)
            consulta = consulta.Where(t => (t.FechaFin == null) == activo.Value);

        return await consulta
            .OrderByDescending(t => t.FechaInicio)
            .ThenBy(t => t.Id)
            .Select(t => new TratamientoResponse(
                t.Id,
                t.ConsultaId,
                t.Consulta.AnimalId,
                t.Consulta.Animal.Nombre,
                t.Medicamento,
                t.Dosis,
                t.FechaInicio,
                t.FechaFin,
                t.Notas,
                t.FechaFin == null))
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task<TratamientoResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken) =>
        db.Tratamientos
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new TratamientoResponse(
                t.Id,
                t.ConsultaId,
                t.Consulta.AnimalId,
                t.Consulta.Animal.Nombre,
                t.Medicamento,
                t.Dosis,
                t.FechaInicio,
                t.FechaFin,
                t.Notas,
                t.FechaFin == null))
            .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<TratamientoResponse> CrearAsync(
        int consultaId, CrearTratamientoRequest request, CancellationToken cancellationToken)
    {
        var consulta = await db.Consultas
            .Include(c => c.Animal)
            .FirstOrDefaultAsync(c => c.Id == consultaId, cancellationToken)
            ?? throw new RecursoNoEncontradoException("No se encontro la consulta indicada.");

        ValidarFechas(request.FechaInicio, request.FechaFin, "FechaInicio", "FechaFin");

        var tratamiento = new Tratamiento
        {
            ConsultaId = consultaId,
            Medicamento = request.Medicamento.Trim(),
            Dosis = request.Dosis.Trim(),
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            Notas = string.IsNullOrWhiteSpace(request.Notas) ? null : request.Notas.Trim()
        };

        consulta.Tratamientos.Add(tratamiento);
        if (tratamiento.FechaFin is null && consulta.Animal is not null)
            consulta.Animal.Estado = "EnTratamiento";

        await db.SaveChangesAsync(cancellationToken);

        return await ObtenerPorIdAsync(tratamiento.Id, cancellationToken)
            ?? throw new InvalidOperationException("No se pudo leer el tratamiento creado.");
    }

    /// <inheritdoc />
    public async Task<TratamientoResponse> CerrarAsync(
        int id, CerrarTratamientoRequest request, CancellationToken cancellationToken)
    {
        var tratamiento = await db.Tratamientos
            .Include(t => t.Consulta)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken)
            ?? throw new RecursoNoEncontradoException("No se encontro el tratamiento indicado.");

        if (tratamiento.FechaFin is not null)
            throw new ReglaNegocioException("El tratamiento ya esta cerrado.");

        var fechaFin = request.FechaFin ?? DateTimeOffset.UtcNow;
        if (fechaFin < tratamiento.FechaInicio)
            throw new ValidacionException("FechaFin", "La fecha de fin no puede ser anterior a la fecha de inicio.");

        tratamiento.FechaFin = fechaFin;
        if (request.Notas is not null)
            tratamiento.Notas = request.Notas.Trim();

        var animalId = tratamiento.Consulta.AnimalId;
        var quedaActivo = await db.Tratamientos
            .AnyAsync(t => t.Consulta.AnimalId == animalId && t.FechaFin == null && t.Id != id, cancellationToken);

        if (!quedaActivo)
        {
            var animal = await db.Animales.FirstOrDefaultAsync(a => a.Id == animalId, cancellationToken);
            if (animal is not null && animal.Estado == "EnTratamiento")
                animal.Estado = "Activo";
        }

        await db.SaveChangesAsync(cancellationToken);

        return await ObtenerPorIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException("No se pudo leer el tratamiento cerrado.");
    }

    private static void ValidarFechas(
        DateTimeOffset fechaInicio, DateTimeOffset? fechaFin, string campoInicio, string campoFin)
    {
        if (fechaInicio > DateTimeOffset.UtcNow)
            throw new ValidacionException(campoInicio, "La fecha de inicio no puede ser futura.");
        if (fechaFin.HasValue && fechaFin.Value < fechaInicio)
            throw new ValidacionException(campoFin, "La fecha de fin no puede ser anterior a la fecha de inicio.");
    }
}
