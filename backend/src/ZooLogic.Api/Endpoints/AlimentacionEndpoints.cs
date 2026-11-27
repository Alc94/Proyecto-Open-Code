using Microsoft.AspNetCore.Http.HttpResults;
using ZooLogic.Api.Contracts.Alimentacion;
using ZooLogic.Api.Contracts.Comunes;
using ZooLogic.Api.Services;
using ZooLogic.Api.Validation;

namespace ZooLogic.Api.Endpoints;

/// <summary>Endpoints de solo lectura del módulo de alimentación.</summary>
public static class AlimentacionEndpoints
{
    /// <summary>Registra los endpoints de alimentación.</summary>
    public static WebApplication MapAlimentacion(this WebApplication app)
    {
        app.MapGet("/api/alimentos", async Task<Ok<IReadOnlyList<AlimentoResponse>>> (
                string? categoria,
                string? texto,
                IAlimentoService servicio,
                CancellationToken cancellationToken) =>
            {
                var alimentos = await servicio.ObtenerAsync(categoria, texto, cancellationToken);
                return TypedResults.Ok(alimentos);
            })
            .WithName("ListarAlimentos")
            .WithSummary("Lista los alimentos disponibles.")
            .WithDescription("Filtra por categoria y por texto del nombre; ordena por nombre con colacion espanola.")
            .Produces<IReadOnlyList<AlimentoResponse>>(StatusCodes.Status200OK)
            .WithTags("Alimentacion");

        app.MapGet("/api/dietas", async Task<Results<Ok<IReadOnlyList<DietaListResponse>>, ValidationProblem>> (
                int? especieId,
                string? estado,
                IDietaService servicio,
                CancellationToken cancellationToken) =>
            {
                var errores = new Dictionary<string, string[]>(StringComparer.Ordinal);

                if (especieId is <= 0)
                {
                    errores[nameof(especieId)] = ["EspecieId debe ser mayor que cero."];
                }

                if (!string.IsNullOrWhiteSpace(estado) && !ValoresDominio.EstadosDieta.Contains(estado))
                {
                    errores[nameof(estado)] =
                        [$"Estado debe ser uno de: {string.Join(", ", ValoresDominio.EstadosDieta)}."];
                }

                if (errores.Count > 0)
                {
                    return TypedResults.ValidationProblem(errores);
                }

                var dietas = await servicio.ObtenerAsync(especieId, estado, cancellationToken);
                return TypedResults.Ok(dietas);
            })
            .WithName("ListarDietas")
            .WithSummary("Lista las dietas registradas.")
            .WithDescription("Filtra por especieId y estado; incluye numero de lineas, kcal totales y si puede aplicarse.")
            .Produces<IReadOnlyList<DietaListResponse>>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .WithTags("Alimentacion");

        app.MapGet("/api/dietas/{id:int}", async Task<Results<Ok<DietaResponse>, NotFound>> (
                int id,
                IDietaService servicio,
                CancellationToken cancellationToken) =>
            {
                var dieta = await servicio.ObtenerPorIdAsync(id, cancellationToken);
                return dieta is null ? TypedResults.NotFound() : TypedResults.Ok(dieta);
            })
            .WithName("ObtenerDieta")
            .WithSummary("Obtiene una dieta con sus lineas.")
            .WithDescription("Devuelve las lineas ordenadas y las kcal aportadas por cada una.")
            .Produces<DietaResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags("Alimentacion");

        app.MapGet("/api/registros-alimentacion", async Task<Results<Ok<PaginaResponse<RegistroAlimentacionResponse>>, ValidationProblem>> (
                int? animalId,
                int? alimentoId,
                DateTimeOffset? desde,
                DateTimeOffset? hasta,
                int? pagina,
                int? tamano,
                IRegistroAlimentacionService servicio,
                CancellationToken cancellationToken) =>
            {
                var paginaActual = pagina ?? 1;
                var tamanoActual = tamano ?? 20;
                var errores = new Dictionary<string, string[]>(StringComparer.Ordinal);

                if (animalId is <= 0)
                {
                    errores[nameof(animalId)] = ["AnimalId debe ser mayor que cero."];
                }

                if (alimentoId is <= 0)
                {
                    errores[nameof(alimentoId)] = ["AlimentoId debe ser mayor que cero."];
                }

                if (paginaActual < 1)
                {
                    errores[nameof(pagina)] = ["Pagina debe ser mayor o igual a 1."];
                }

                if (tamanoActual is < 1 or > 100)
                {
                    errores[nameof(tamano)] = ["Tamano debe estar entre 1 y 100."];
                }

                if (desde is not null && hasta is not null && desde > hasta)
                {
                    errores[nameof(desde)] = ["Desde no puede ser posterior a hasta."];
                }

                if (errores.Count > 0)
                {
                    return TypedResults.ValidationProblem(errores);
                }

                var registros = await servicio.ObtenerAsync(
                    animalId, alimentoId, desde, hasta, paginaActual, tamanoActual, cancellationToken);
                return TypedResults.Ok(registros);
            })
            .WithName("ListarRegistrosAlimentacion")
            .WithSummary("Lista los registros de alimentacion de forma paginada.")
            .WithDescription("Filtra por animalId, alimentoId y rango de fechas desde/hasta; ordena por fecha descendente.")
            .Produces<PaginaResponse<RegistroAlimentacionResponse>>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .WithTags("Alimentacion");

        return app;
    }
}
