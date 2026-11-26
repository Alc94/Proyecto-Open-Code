using Microsoft.EntityFrameworkCore;
using ZooLogic.Api.Contracts.Alimentacion;
using ZooLogic.Api.Contracts.Animales;
using ZooLogic.Api.Contracts.Alertas;
using ZooLogic.Api.Contracts.Comunes;
using ZooLogic.Api.Contracts.Consultas;
using ZooLogic.Api.Data;
using ZooLogic.Api.Exceptions;
using ZooLogic.Api.Models;

namespace ZooLogic.Api.Services;

internal sealed class AnimalService(ZooDbContext db) : IAnimalService
{
    public async Task<PaginaResponse<AnimalListResponse>> ObtenerAsync(
        string? estado,
        int? especieId,
        string? recinto,
        string? texto,
        int pagina,
        int tamano,
        CancellationToken cancellationToken)
    {
        var consulta = db.Animales.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(estado))
        {
            consulta = consulta.Where(animal => animal.Estado == estado);
        }

        if (especieId is > 0)
        {
            consulta = consulta.Where(animal => animal.EspecieId == especieId.Value);
        }

        if (!string.IsNullOrWhiteSpace(recinto))
        {
            var patron = $"%{EscaparLike(recinto.Trim())}%";
            consulta = consulta.Where(animal => EF.Functions.ILike(animal.Recinto, patron));
        }

        if (!string.IsNullOrWhiteSpace(texto))
        {
            var patron = $"%{EscaparLike(texto.Trim())}%";
            consulta = consulta.Where(animal =>
                EF.Functions.ILike(animal.Nombre, patron) ||
                EF.Functions.ILike(animal.Especie.NombreComun, patron));
        }

        var total = await consulta.CountAsync(cancellationToken);

        var elementos = await consulta
            .OrderBy(animal => animal.Id)
            .Skip((pagina - 1) * tamano)
            .Take(tamano)
            .Select(animal => new AnimalListResponse(
                animal.Id,
                animal.Nombre,
                animal.EspecieId,
                animal.Especie.NombreComun,
                animal.Sexo,
                animal.FechaNacimiento,
                0,
                animal.PesoKg,
                animal.Recinto,
                animal.Estado,
                animal.FotoUrl,
                animal.Alertas.Count(alerta => alerta.Estado == "Abierta"),
                animal.Alertas.Any(alerta =>
                    alerta.Estado == "Abierta" &&
                    (alerta.Gravedad == "Alta" || alerta.Gravedad == "Critica"))))
            .ToListAsync(cancellationToken);

        var respuesta = elementos
            .Select(animal => animal with { EdadAnios = CalcularEdad(animal.FechaNacimiento) })
            .ToList();

        return new PaginaResponse<AnimalListResponse>(respuesta, total, pagina, tamano);
    }

    public async Task<AnimalDetalleResponse?> ObtenerPorIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var animal = await db.Animales
            .AsNoTracking()
            .Where(animal => animal.Id == id)
            .Select(animal => new AnimalDetalleResponse(
                animal.Id,
                animal.Nombre,
                animal.EspecieId,
                new EspecieResumenResponse(
                    animal.Especie.Id,
                    animal.Especie.NombreComun,
                    animal.Especie.NombreCientifico,
                    animal.Especie.Grupo,
                    animal.Especie.TipoDieta),
                animal.Sexo,
                animal.FechaNacimiento,
                0,
                animal.PesoKg,
                animal.Recinto,
                animal.Estado,
                animal.FotoUrl,
                animal.Alertas.Count(alerta => alerta.Estado == "Abierta"),
                animal.Alertas.Any(alerta =>
                    alerta.Estado == "Abierta" &&
                    (alerta.Gravedad == "Alta" || alerta.Gravedad == "Critica")),
                animal.Consultas
                    .SelectMany(consulta => consulta.Tratamientos)
                    .Count(tratamiento => tratamiento.FechaFin == null),
                animal.Consultas
                    .OrderByDescending(consulta => consulta.Fecha)
                    .Select(consulta => new ConsultaResumenResponse(
                        consulta.Id,
                        consulta.Fecha,
                        consulta.Veterinario,
                        consulta.Motivo,
                        consulta.Diagnostico))
                    .FirstOrDefault(),
                db.Dietas
                    .Where(dieta =>
                        dieta.EspecieId == animal.EspecieId &&
                        dieta.Estado == "Validada")
                    .OrderByDescending(dieta => dieta.Id)
                    .Select(dieta => (int?)dieta.Id)
                    .FirstOrDefault()))
            .FirstOrDefaultAsync(cancellationToken);

        return animal is null
            ? null
            : animal with { EdadAnios = CalcularEdad(animal.FechaNacimiento) };
    }

    public async Task<HistorialAnimalResponse?> ObtenerHistorialAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var animal = await db.Animales
            .AsNoTracking()
            .Where(animal => animal.Id == id)
            .Select(animal => new AnimalResumenResponse(
                animal.Id,
                animal.Nombre,
                animal.Especie.NombreComun,
                animal.Recinto))
            .FirstOrDefaultAsync(cancellationToken);

        if (animal is null)
        {
            return null;
        }

        var consultas = await db.Consultas
            .AsNoTracking()
            .Where(consulta => consulta.AnimalId == id)
            .OrderByDescending(consulta => consulta.Fecha)
            .Select(consulta => new ConsultaDetalleResponse(
                consulta.Id,
                consulta.AnimalId,
                consulta.Animal.Nombre,
                consulta.Fecha,
                consulta.Veterinario,
                consulta.Motivo,
                consulta.Diagnostico,
                consulta.PesoKg,
                consulta.Temperatura,
                consulta.Tratamientos
                    .OrderBy(tratamiento => tratamiento.FechaInicio)
                    .Select(tratamiento => new TratamientoResponse(
                        tratamiento.Id,
                        tratamiento.ConsultaId,
                        consulta.AnimalId,
                        consulta.Animal.Nombre,
                        tratamiento.Medicamento,
                        tratamiento.Dosis,
                        tratamiento.FechaInicio,
                        tratamiento.FechaFin,
                        tratamiento.Notas,
                        tratamiento.FechaFin == null))
                    .ToList()))
            .ToListAsync(cancellationToken);

        var alertas = await db.AlertasSalud
            .AsNoTracking()
            .Where(alerta => alerta.AnimalId == id)
            .OrderByDescending(alerta => alerta.Fecha)
            .Select(alerta => new AlertaResponse(
                alerta.Id,
                alerta.AnimalId,
                alerta.Animal.Nombre,
                alerta.Animal.Especie.NombreComun,
                alerta.Fecha,
                alerta.Gravedad,
                alerta.Tipo,
                alerta.Mensaje,
                alerta.Estado,
                alerta.Origen,
                alerta.Gravedad == "Alta" || alerta.Gravedad == "Critica",
                null))
            .ToListAsync(cancellationToken);

        var alimentacion = await db.RegistrosAlimentacion
            .AsNoTracking()
            .Where(registro => registro.AnimalId == id)
            .OrderByDescending(registro => registro.Fecha)
            .Select(registro => new RegistroAlimentacionResponse(
                registro.Id,
                registro.AnimalId,
                registro.Animal.Nombre,
                registro.AlimentoId,
                registro.Alimento.Nombre,
                registro.Fecha,
                registro.CantidadKg,
                registro.Consumido,
                registro.CantidadKg * registro.Alimento.KcalPorKg,
                db.Dietas.Any(dieta =>
                    dieta.EspecieId == registro.Animal.EspecieId &&
                    dieta.Estado == "Validada" &&
                    dieta.Lineas.Any(linea => linea.AlimentoId == registro.AlimentoId))))
            .ToListAsync(cancellationToken);

        return new HistorialAnimalResponse(animal, consultas, alertas, alimentacion);
    }

    public async Task<DietaResponse?> ObtenerDietaAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var especieId = await db.Animales
            .AsNoTracking()
            .Where(animal => animal.Id == id)
            .Select(animal => (int?)animal.EspecieId)
            .FirstOrDefaultAsync(cancellationToken);

        if (especieId is null)
        {
            return null;
        }

        return await db.Dietas
            .AsNoTracking()
            .Where(dieta =>
                dieta.EspecieId == especieId.Value &&
                dieta.Estado == "Validada")
            .OrderByDescending(dieta => dieta.Id)
            .Select(dieta => new DietaResponse(
                dieta.Id,
                dieta.EspecieId,
                dieta.Especie.NombreComun,
                dieta.Nombre,
                dieta.Estado,
                dieta.ValidadaPor,
                dieta.Origen,
                dieta.Lineas.Count,
                dieta.Lineas.Sum(linea =>
                    linea.CantidadKgDia * linea.Alimento.KcalPorKg),
                dieta.Estado == "Validada",
                dieta.Lineas
                    .OrderBy(linea => linea.Id)
                    .Select(linea => new DietaLineaResponse(
                        linea.Id,
                        linea.AlimentoId,
                        linea.Alimento.Nombre,
                        linea.Alimento.Categoria,
                        linea.CantidadKgDia,
                        linea.Frecuencia,
                        linea.CantidadKgDia * linea.Alimento.KcalPorKg))
                    .ToList()))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AnimalDetalleResponse> CrearAsync(
        CrearAnimalRequest request,
        CancellationToken cancellationToken)
    {
        ValidarFechaNacimiento(request.FechaNacimiento);
        await ValidarEspecieAsync(request.EspecieId, cancellationToken);

        var animal = new Animal
        {
            Nombre = request.Nombre.Trim(),
            EspecieId = request.EspecieId,
            Sexo = request.Sexo,
            FechaNacimiento = request.FechaNacimiento,
            PesoKg = request.PesoKg,
            Recinto = request.Recinto.Trim(),
            Estado = string.IsNullOrWhiteSpace(request.Estado) ? "Activo" : request.Estado,
            FotoUrl = string.IsNullOrWhiteSpace(request.FotoUrl) ? null : request.FotoUrl.Trim()
        };

        db.Animales.Add(animal);
        await db.SaveChangesAsync(cancellationToken);

        return await ObtenerPorIdAsync(animal.Id, cancellationToken)
            ?? throw new InvalidOperationException("No se pudo recuperar el animal creado.");
    }

    public async Task<AnimalDetalleResponse?> ActualizarAsync(
        int id,
        ActualizarAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var animal = await db.Animales
            .FirstOrDefaultAsync(animal => animal.Id == id, cancellationToken);

        if (animal is null)
        {
            return null;
        }

        ValidarFechaNacimiento(request.FechaNacimiento);
        await ValidarEspecieAsync(request.EspecieId, cancellationToken);

        if (request.Estado == "Activo")
        {
            var tieneTratamientoActivo = await db.Tratamientos
                .AnyAsync(
                    tratamiento =>
                        tratamiento.Consulta.AnimalId == id &&
                        tratamiento.FechaFin == null,
                    cancellationToken);

            if (tieneTratamientoActivo)
            {
                throw new ReglaNegocioException(
                    "No se puede poner al animal en estado Activo mientras tenga un tratamiento activo.");
            }
        }

        animal.Nombre = request.Nombre.Trim();
        animal.EspecieId = request.EspecieId;
        animal.Sexo = request.Sexo;
        animal.FechaNacimiento = request.FechaNacimiento;
        animal.PesoKg = request.PesoKg;
        animal.Recinto = request.Recinto.Trim();
        animal.Estado = request.Estado;
        animal.FotoUrl = string.IsNullOrWhiteSpace(request.FotoUrl) ? null : request.FotoUrl.Trim();

        await db.SaveChangesAsync(cancellationToken);

        return await ObtenerPorIdAsync(animal.Id, cancellationToken);
    }

    private async Task ValidarEspecieAsync(int especieId, CancellationToken cancellationToken)
    {
        if (!await db.Especies.AnyAsync(especie => especie.Id == especieId, cancellationToken))
        {
            throw new ValidacionException(
                nameof(CrearAnimalRequest.EspecieId),
                "La especie indicada no existe.");
        }
    }

    private static void ValidarFechaNacimiento(DateTimeOffset fechaNacimiento)
    {
        if (fechaNacimiento > DateTimeOffset.UtcNow)
        {
            throw new ValidacionException(
                nameof(CrearAnimalRequest.FechaNacimiento),
                "La fecha de nacimiento no puede ser futura.");
        }
    }

    private static int CalcularEdad(DateTimeOffset fechaNacimiento)
    {
        var hoy = DateTimeOffset.UtcNow;
        var edad = hoy.Year - fechaNacimiento.Year;

        if (fechaNacimiento.Date > hoy.Date.AddYears(-edad))
        {
            edad--;
        }

        return Math.Max(0, edad);
    }

    private static string EscaparLike(string valor) =>
        valor.Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("%", "\\%", StringComparison.Ordinal)
            .Replace("_", "\\_", StringComparison.Ordinal);
}
