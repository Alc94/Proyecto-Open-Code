namespace ZooLogic.Api.Contracts.Comunes;

/// <summary>Representa una página de resultados de una consulta paginada.</summary>
/// <param name="Elementos">Elementos de la página actual.</param>
/// <param name="Total">Número total de elementos que cumplen el filtro.</param>
/// <param name="Pagina">Número de página solicitado, empezando en 1.</param>
/// <param name="TamanoPagina">Cantidad máxima de elementos por página.</param>
/// <typeparam name="T">Tipo de los elementos de la página.</typeparam>
public sealed record PaginaResponse<T>(
    IReadOnlyList<T> Elementos,
    int Total,
    int Pagina,
    int TamanoPagina);
