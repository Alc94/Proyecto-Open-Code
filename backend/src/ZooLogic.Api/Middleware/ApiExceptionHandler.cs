using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ZooLogic.Api.Exceptions;

namespace ZooLogic.Api.Middleware;

/// <summary>
/// Traduce las excepciones de dominio conocidas a respuestas RFC 7807 (Problem Details).
/// Las excepciones no reconocidas se dejan al manejador por defecto (500).
/// </summary>
internal sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title, detail) = exception switch
        {
            RecursoNoEncontradoException ex =>
                (StatusCodes.Status404NotFound, "Recurso no encontrado", ex.Message),
            ReglaNegocioException ex =>
                (StatusCodes.Status409Conflict, "Conflicto con las reglas de negocio", ex.Message),
            DbUpdateException ex when EsConflictoDeUnicidad(ex) =>
                (StatusCodes.Status409Conflict, "Conflicto de unicidad",
                    "Ya existe un registro con esos datos."),
            _ => (0, string.Empty, string.Empty)
        };

        if (statusCode == 0)
        {
            return false;
        }

        // Los mensajes de las excepciones de dominio son seguros por diseño;
        // nunca se expone el mensaje de una excepción interna.
        logger.LogWarning(exception, "Excepción de API controlada: {Title}", title);

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            },
            cancellationToken);

        return true;
    }

    private static bool EsConflictoDeUnicidad(DbUpdateException exception) =>
        exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation };
}
