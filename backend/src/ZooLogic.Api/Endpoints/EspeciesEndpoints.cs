using Microsoft.AspNetCore.Http.HttpResults;
using ZooLogic.Api.Contracts.Especies;
using ZooLogic.Api.Services;
using ZooLogic.Api.Validation;

namespace ZooLogic.Api.Endpoints;

public static class EspeciesEndpoints
{
    public static WebApplication MapEspecies(this WebApplication app)
    {
        app.MapGet("/api/especies", async Task<Results<Ok<IReadOnlyList<EspecieResponse>>, ValidationProblem>> (
                string? grupo,
                string? tipoDieta,
                string? texto,
                IEspecieService service,
                CancellationToken cancellationToken) =>
            {
                var errores = ValidarFiltros(grupo, tipoDieta);
                if (errores.Count > 0)
                {
                    return TypedResults.ValidationProblem(errores);
                }

                var especies = await service.ObtenerAsync(grupo, tipoDieta, texto, cancellationToken);
                return TypedResults.Ok(especies);
            })
            .WithName("ListarEspecies")
            .WithSummary("Lista las especies del zoológico")
            .WithDescription(
                "Devuelve el catálogo de especies con el número de animales y dietas asociados. " +
                "Permite filtrar por grupo, tipo de dieta y texto del nombre común o científico.")
            .Produces<IReadOnlyList<EspecieResponse>>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .WithTags("Animales");

        app.MapGet("/api/especies/{id:int}", async Task<Results<Ok<EspecieResponse>, NotFound>> (
                int id,
                IEspecieService service,
                CancellationToken cancellationToken) =>
            {
                var especie = await service.ObtenerPorIdAsync(id, cancellationToken);
                return especie is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(especie);
            })
            .WithName("ObtenerEspecie")
            .WithSummary("Obtiene una especie por su identificador")
            .WithDescription(
                "Devuelve los datos de la especie y los conteos de animales y dietas. " +
                "Responde 404 si la especie no existe.")
            .Produces<EspecieResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags("Animales");

        return app;
    }

    private static Dictionary<string, string[]> ValidarFiltros(string? grupo, string? tipoDieta)
    {
        var errores = new Dictionary<string, string[]>(StringComparer.Ordinal);

        if (!string.IsNullOrWhiteSpace(grupo) && !ValoresDominio.GruposEspecie.Contains(grupo))
        {
            errores[nameof(grupo)] =
                [$"Valores permitidos: {string.Join(", ", ValoresDominio.GruposEspecie)}."];
        }

        if (!string.IsNullOrWhiteSpace(tipoDieta) && !ValoresDominio.TiposDieta.Contains(tipoDieta))
        {
            errores[nameof(tipoDieta)] =
                [$"Valores permitidos: {string.Join(", ", ValoresDominio.TiposDieta)}."];
        }

        return errores;
    }
}
