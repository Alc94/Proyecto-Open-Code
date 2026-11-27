using Microsoft.AspNetCore.Http.HttpResults;
using ZooLogic.Api.Contracts.Alertas;
using ZooLogic.Api.Contracts.Comunes;
using ZooLogic.Api.Contracts.Consultas;
using ZooLogic.Api.Services;
using ZooLogic.Api.Validation;

namespace ZooLogic.Api.Endpoints;

public static class VeterinariaEndpoints
{
    public static WebApplication MapVeterinaria(this WebApplication app)
    {
        app.MapGet("/api/consultas", async Task<Results<Ok<PaginaResponse<ConsultaResumenResponse>>, ValidationProblem>> (
                int? animalId,
                DateTimeOffset? desde,
                DateTimeOffset? hasta,
                int? pagina,
                int? tamano,
                IConsultaService service,
                CancellationToken cancellationToken) =>
            {
                var errores = ValidarFiltrosConsulta(animalId, desde, hasta, pagina, tamano);
                if (errores.Count > 0)
                {
                    return TypedResults.ValidationProblem(errores);
                }

                var resultado = await service.ObtenerAsync(
                    animalId,
                    desde,
                    hasta,
                    pagina ?? 1,
                    tamano ?? 20,
                    cancellationToken);

                return TypedResults.Ok(resultado);
            })
            .WithName("ListarConsultas")
            .WithSummary("Lista las consultas veterinarias")
            .WithDescription(
                "Devuelve una página de consultas con filtros por animal y rango de fechas. " +
                "Ordena por fecha descendente y luego por identificador.")
            .Produces<PaginaResponse<ConsultaResumenResponse>>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .WithTags("Veterinaria");

        app.MapGet("/api/consultas/{id:int}", async Task<Results<Ok<ConsultaDetalleResponse>, NotFound>> (
                int id,
                IConsultaService service,
                CancellationToken cancellationToken) =>
            {
                var consulta = await service.ObtenerPorIdAsync(id, cancellationToken);
                return consulta is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(consulta);
            })
            .WithName("ObtenerConsulta")
            .WithSummary("Obtiene una consulta veterinaria")
            .WithDescription(
                "Devuelve la consulta con el nombre del animal y sus tratamientos ordenados por fecha de inicio. " +
                "Responde 404 si la consulta no existe.")
            .Produces<ConsultaDetalleResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags("Veterinaria");

        app.MapPost("/api/consultas", async Task<Created<ConsultaDetalleResponse>> (
                CrearConsultaRequest request,
                IConsultaService service,
                CancellationToken cancellationToken) =>
            {
                var consulta = await service.CrearAsync(request, cancellationToken);
                return TypedResults.Created($"/api/consultas/{consulta.Id}", consulta);
            })
            .WithName("CrearConsulta")
            .WithSummary("Crea una consulta veterinaria")
            .WithDescription(
                "Crea una consulta y, opcionalmente, sus tratamientos anidados. " +
                "Si alguno de los tratamientos está activo, el animal pasa a EnTratamiento. " +
                "Devuelve 201 con la cabecera Location.")
            .Produces<ConsultaDetalleResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict)
            .WithTags("Veterinaria");

        app.MapPost("/api/consultas/{id:int}/tratamientos", async Task<Created<TratamientoResponse>> (
                int id,
                CrearTratamientoRequest request,
                ITratamientoService service,
                CancellationToken cancellationToken) =>
            {
                var tratamiento = await service.CrearAsync(id, request, cancellationToken);
                return TypedResults.Created($"/api/tratamientos/{tratamiento.Id}", tratamiento);
            })
            .WithName("CrearTratamiento")
            .WithSummary("Añade un tratamiento a una consulta")
            .WithDescription(
                "Añade un tratamiento a una consulta existente. Si el tratamiento está activo, " +
                "el animal pasa a EnTratamiento. Devuelve 201 con la cabecera Location.")
            .Produces<TratamientoResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .WithTags("Veterinaria");

        app.MapGet("/api/tratamientos", async Task<Results<Ok<IReadOnlyList<TratamientoResponse>>, ValidationProblem>> (
                int? animalId,
                bool? activo,
                ITratamientoService service,
                CancellationToken cancellationToken) =>
            {
                var errores = new Dictionary<string, string[]>(StringComparer.Ordinal);
                if (animalId is <= 0)
                {
                    errores["animalId"] = ["Debe ser un identificador positivo."];
                }

                if (errores.Count > 0)
                {
                    return TypedResults.ValidationProblem(errores);
                }

                var tratamientos = await service.ObtenerAsync(animalId, activo, cancellationToken);
                return TypedResults.Ok(tratamientos);
            })
            .WithName("ListarTratamientos")
            .WithSummary("Lista los tratamientos veterinarios")
            .WithDescription(
                "Devuelve los tratamientos con filtros opcionales por animal y por estado activo. " +
                "Un tratamiento activo es el que no tiene fecha de fin.")
            .Produces<IReadOnlyList<TratamientoResponse>>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .WithTags("Veterinaria");

        app.MapGet("/api/tratamientos/{id:int}", async Task<Results<Ok<TratamientoResponse>, NotFound>> (
                int id,
                ITratamientoService service,
                CancellationToken cancellationToken) =>
            {
                var tratamiento = await service.ObtenerPorIdAsync(id, cancellationToken);
                return tratamiento is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(tratamiento);
            })
            .WithName("ObtenerTratamiento")
            .WithSummary("Obtiene un tratamiento")
            .WithDescription("Devuelve un tratamiento por su identificador. Responde 404 si no existe.")
            .Produces<TratamientoResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags("Veterinaria");

        app.MapPatch("/api/tratamientos/{id:int}/cerrar", async Task<Ok<TratamientoResponse>> (
                int id,
                CerrarTratamientoRequest request,
                ITratamientoService service,
                CancellationToken cancellationToken) =>
            {
                var tratamiento = await service.CerrarAsync(id, request, cancellationToken);
                return TypedResults.Ok(tratamiento);
            })
            .WithName("CerrarTratamiento")
            .WithSummary("Cierra un tratamiento")
            .WithDescription(
                "Fija la fecha de fin del tratamiento. Si era el último tratamiento activo del animal, " +
                "el animal vuelve a Activo. No borra el historial clínico.")
            .Produces<TratamientoResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .WithTags("Veterinaria");

        app.MapGet("/api/alertas", async Task<Results<Ok<IReadOnlyList<AlertaResponse>>, ValidationProblem>> (
                string? estado,
                string? gravedad,
                int? animalId,
                IAlertaService service,
                CancellationToken cancellationToken) =>
            {
                var errores = ValidarFiltrosAlerta(estado, gravedad, animalId);
                if (errores.Count > 0)
                {
                    return TypedResults.ValidationProblem(errores);
                }

                var alertas = await service.ObtenerAsync(estado, gravedad, animalId, cancellationToken);
                return TypedResults.Ok(alertas);
            })
            .WithName("ListarAlertas")
            .WithSummary("Lista las alertas de salud")
            .WithDescription(
                "Devuelve las alertas ordenadas por gravedad (Critica, Alta, Media, Baja) y luego por fecha " +
                "descendente. Las alertas de gravedad Alta o Critica llegan marcadas como destacadas.")
            .Produces<IReadOnlyList<AlertaResponse>>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .WithTags("Veterinaria");

        app.MapGet("/api/alertas/{id:int}", async Task<Results<Ok<AlertaResponse>, NotFound>> (
                int id,
                IAlertaService service,
                CancellationToken cancellationToken) =>
            {
                var alerta = await service.ObtenerPorIdAsync(id, cancellationToken);
                return alerta is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(alerta);
            })
            .WithName("ObtenerAlerta")
            .WithSummary("Obtiene una alerta de salud")
            .WithDescription("Devuelve una alerta por su identificador. Responde 404 si no existe.")
            .Produces<AlertaResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags("Veterinaria");

        app.MapPatch("/api/alertas/{id:int}/estado", async Task<Ok<AlertaResponse>> (
                int id,
                CambiarEstadoAlertaRequest request,
                IAlertaService service,
                CancellationToken cancellationToken) =>
            {
                var alerta = await service.CambiarEstadoAsync(id, request, cancellationToken);
                return TypedResults.Ok(alerta);
            })
            .WithName("CambiarEstadoAlerta")
            .WithSummary("Cambia el estado de una alerta")
            .WithDescription(
                "Permite únicamente las transiciones Abierta a Revisada y Revisada a Cerrada. " +
                "Cualquier otra transición responde 409.")
            .Produces<AlertaResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .WithTags("Veterinaria");

        return app;
    }

    private static Dictionary<string, string[]> ValidarFiltrosConsulta(
        int? animalId, DateTimeOffset? desde, DateTimeOffset? hasta, int? pagina, int? tamano)
    {
        var errores = new Dictionary<string, string[]>(StringComparer.Ordinal);

        if (animalId is <= 0)
        {
            errores["animalId"] = ["Debe ser un identificador positivo."];
        }

        if (desde is not null && hasta is not null && hasta < desde)
        {
            errores["hasta"] = ["No puede ser anterior a desde."];
        }

        if (pagina is <= 0)
        {
            errores["pagina"] = ["Debe ser mayor o igual que 1."];
        }

        if (tamano is < 1 or > 100)
        {
            errores["tamano"] = ["Debe estar entre 1 y 100."];
        }

        return errores;
    }

    private static Dictionary<string, string[]> ValidarFiltrosAlerta(
        string? estado, string? gravedad, int? animalId)
    {
        var errores = new Dictionary<string, string[]>(StringComparer.Ordinal);

        if (estado is not null && !ValoresDominio.EstadosAlerta.Contains(estado))
        {
            errores[nameof(estado)] =
                [$"Valores permitidos: {string.Join(", ", ValoresDominio.EstadosAlerta)}."];
        }

        if (gravedad is not null && !ValoresDominio.GravedadesAlerta.Contains(gravedad))
        {
            errores[nameof(gravedad)] =
                [$"Valores permitidos: {string.Join(", ", ValoresDominio.GravedadesAlerta)}."];
        }

        if (animalId is <= 0)
        {
            errores["animalId"] = ["Debe ser un identificador positivo."];
        }

        return errores;
    }
}
