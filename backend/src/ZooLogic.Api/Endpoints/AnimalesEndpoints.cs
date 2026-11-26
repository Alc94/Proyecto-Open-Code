using Microsoft.AspNetCore.Http.HttpResults;
using ZooLogic.Api.Contracts.Alimentacion;
using ZooLogic.Api.Contracts.Animales;
using ZooLogic.Api.Contracts.Comunes;
using ZooLogic.Api.Services;
using ZooLogic.Api.Validation;

namespace ZooLogic.Api.Endpoints;

public static class AnimalesEndpoints
{
    public static WebApplication MapAnimales(this WebApplication app)
    {
        app.MapGet("/api/animales", async Task<Results<Ok<PaginaResponse<AnimalListResponse>>, ValidationProblem>> (
                string? estado,
                int? especieId,
                string? recinto,
                string? texto,
                int? pagina,
                int? tamano,
                IAnimalService service,
                CancellationToken cancellationToken) =>
            {
                var errores = ValidarFiltros(estado, especieId, pagina, tamano);
                if (errores.Count > 0)
                {
                    return TypedResults.ValidationProblem(errores);
                }

                var resultado = await service.ObtenerAsync(
                    estado,
                    especieId,
                    recinto,
                    texto,
                    pagina ?? 1,
                    tamano ?? 20,
                    cancellationToken);

                return TypedResults.Ok(resultado);
            })
            .WithName("ListarAnimales")
            .WithSummary("Lista los animales del zoológico")
            .WithDescription(
                "Devuelve una página de animales con filtros por estado, especie, recinto y texto del nombre. " +
                "Incluye el número de alertas abiertas y si tiene alguna alerta grave.")
            .Produces<PaginaResponse<AnimalListResponse>>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .WithTags("Animales");

        app.MapGet("/api/animales/{id:int}", async Task<Results<Ok<AnimalDetalleResponse>, NotFound>> (
                int id,
                IAnimalService service,
                CancellationToken cancellationToken) =>
            {
                var animal = await service.ObtenerPorIdAsync(id, cancellationToken);
                return animal is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(animal);
            })
            .WithName("ObtenerAnimal")
            .WithSummary("Obtiene la ficha de un animal")
            .WithDescription(
                "Devuelve los datos del animal, su especie, sus alertas abiertas, " +
                "sus tratamientos activos, su última consulta y la dieta validada de su especie. " +
                "Responde 404 si el animal no existe.")
            .Produces<AnimalDetalleResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags("Animales");

        app.MapGet("/api/animales/{id:int}/historial", async Task<Results<Ok<HistorialAnimalResponse>, NotFound>> (
                int id,
                IAnimalService service,
                CancellationToken cancellationToken) =>
            {
                var historial = await service.ObtenerHistorialAsync(id, cancellationToken);
                return historial is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(historial);
            })
            .WithName("ObtenerHistorialAnimal")
            .WithSummary("Obtiene el historial de un animal")
            .WithDescription(
                "Devuelve las consultas veterinarias con sus tratamientos, las alertas de salud " +
                "y los registros de alimentación del animal. Responde 404 si el animal no existe.")
            .Produces<HistorialAnimalResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags("Animales");

        app.MapGet("/api/animales/{id:int}/dieta", async Task<Results<Ok<DietaResponse>, NotFound>> (
                int id,
                IAnimalService service,
                CancellationToken cancellationToken) =>
            {
                var dieta = await service.ObtenerDietaAsync(id, cancellationToken);
                return dieta is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(dieta);
            })
            .WithName("ObtenerDietaAnimal")
            .WithSummary("Obtiene la dieta validada de la especie de un animal")
            .WithDescription(
                "Devuelve la dieta validada más reciente de la especie del animal. " +
                "Responde 404 si el animal no existe o su especie no tiene una dieta validada.")
            .Produces<DietaResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags("Animales");

        app.MapPost("/api/animales", async Task<Results<Created<AnimalDetalleResponse>, ValidationProblem>> (
                CrearAnimalRequest request,
                IAnimalService service,
                CancellationToken cancellationToken) =>
            {
                var animal = await service.CrearAsync(request, cancellationToken);
                return TypedResults.Created($"/api/animales/{animal.Id}", animal);
            })
            .WithName("CrearAnimal")
            .WithSummary("Crea un animal")
            .WithDescription(
                "Crea un animal en la especie indicada. La fecha de nacimiento no puede ser futura " +
                "y la especie debe existir. Devuelve 201 con la cabecera Location.")
            .Produces<AnimalDetalleResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict)
            .WithTags("Animales");

        app.MapPut("/api/animales/{id:int}", async Task<Results<Ok<AnimalDetalleResponse>, NotFound>> (
                int id,
                ActualizarAnimalRequest request,
                IAnimalService service,
                CancellationToken cancellationToken) =>
            {
                var animal = await service.ActualizarAsync(id, request, cancellationToken);
                return animal is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(animal);
            })
            .WithName("ActualizarAnimal")
            .WithSummary("Actualiza un animal")
            .WithDescription(
                "Actualiza todos los datos editables del animal. Rechaza con 409 pasar a Activo " +
                "si el animal tiene un tratamiento activo. Responde 404 si el animal no existe.")
            .Produces<AnimalDetalleResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .WithTags("Animales");

        return app;
    }

    private static Dictionary<string, string[]> ValidarFiltros(
        string? estado,
        int? especieId,
        int? pagina,
        int? tamano)
    {
        var errores = new Dictionary<string, string[]>(StringComparer.Ordinal);

        if (!string.IsNullOrWhiteSpace(estado) && !ValoresDominio.EstadosAnimal.Contains(estado))
        {
            errores[nameof(estado)] =
                [$"Valores permitidos: {string.Join(", ", ValoresDominio.EstadosAnimal)}."];
        }

        if (especieId is <= 0)
        {
            errores[nameof(especieId)] = ["El identificador de especie debe ser mayor que cero."];
        }

        if (pagina is < 1)
        {
            errores[nameof(pagina)] = ["La página debe ser mayor o igual que 1."];
        }

        if (tamano is < 1 or > 100)
        {
            errores[nameof(tamano)] = ["El tamaño de página debe estar entre 1 y 100."];
        }

        return errores;
    }
}
