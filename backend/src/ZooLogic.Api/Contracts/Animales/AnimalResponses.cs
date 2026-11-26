using ZooLogic.Api.Contracts.Alimentacion;
using ZooLogic.Api.Contracts.Alertas;
using ZooLogic.Api.Contracts.Comunes;
using ZooLogic.Api.Contracts.Consultas;

namespace ZooLogic.Api.Contracts.Animales;

/// <summary>Resumen mínimo de un animal para incrustar en otros recursos.</summary>
/// <param name="Id">Identificador del animal.</param>
/// <param name="Nombre">Nombre propio del animal.</param>
/// <param name="EspecieNombreComun">Nombre común de su especie.</param>
/// <param name="Recinto">Recinto en el que vive.</param>
public sealed record AnimalResumenResponse(
    int Id,
    string Nombre,
    string EspecieNombreComun,
    string Recinto);

/// <summary>Animal tal como aparece en el listado paginado.</summary>
/// <param name="Id">Identificador del animal.</param>
/// <param name="Nombre">Nombre propio del animal.</param>
/// <param name="EspecieId">Identificador de la especie.</param>
/// <param name="EspecieNombreComun">Nombre común de la especie.</param>
/// <param name="Sexo">Sexo del animal.</param>
/// <param name="FechaNacimiento">Fecha de nacimiento.</param>
/// <param name="EdadAnios">Edad calculada en años cumplidos.</param>
/// <param name="PesoKg">Peso actual en kilogramos.</param>
/// <param name="Recinto">Recinto en el que vive.</param>
/// <param name="Estado">Estado actual del animal.</param>
/// <param name="FotoUrl">URL de la fotografía, si existe.</param>
/// <param name="AlertasAbiertas">Número de alertas de salud abiertas.</param>
/// <param name="TieneAlertaGrave">Indica si tiene alguna alerta abierta de gravedad alta o crítica.</param>
public sealed record AnimalListResponse(
    int Id,
    string Nombre,
    int EspecieId,
    string EspecieNombreComun,
    string Sexo,
    DateTimeOffset FechaNacimiento,
    int EdadAnios,
    decimal PesoKg,
    string Recinto,
    string Estado,
    string? FotoUrl,
    int AlertasAbiertas,
    bool TieneAlertaGrave);

/// <summary>Ficha completa de un animal.</summary>
/// <param name="Id">Identificador del animal.</param>
/// <param name="Nombre">Nombre propio del animal.</param>
/// <param name="EspecieId">Identificador de la especie.</param>
/// <param name="Especie">Datos resumidos de la especie.</param>
/// <param name="Sexo">Sexo del animal.</param>
/// <param name="FechaNacimiento">Fecha de nacimiento.</param>
/// <param name="EdadAnios">Edad calculada en años cumplidos.</param>
/// <param name="PesoKg">Peso actual en kilogramos.</param>
/// <param name="Recinto">Recinto en el que vive.</param>
/// <param name="Estado">Estado actual del animal.</param>
/// <param name="FotoUrl">URL de la fotografía, si existe.</param>
/// <param name="AlertasAbiertas">Número de alertas de salud abiertas.</param>
/// <param name="TieneAlertaGrave">Indica si tiene alguna alerta abierta de gravedad alta o crítica.</param>
/// <param name="TratamientosActivos">Número de tratamientos sin fecha de fin.</param>
/// <param name="UltimaConsulta">Última consulta veterinaria, si existe.</param>
/// <param name="DietaValidadaId">Identificador de la dieta validada de su especie, si existe.</param>
public sealed record AnimalDetalleResponse(
    int Id,
    string Nombre,
    int EspecieId,
    EspecieResumenResponse Especie,
    string Sexo,
    DateTimeOffset FechaNacimiento,
    int EdadAnios,
    decimal PesoKg,
    string Recinto,
    string Estado,
    string? FotoUrl,
    int AlertasAbiertas,
    bool TieneAlertaGrave,
    int TratamientosActivos,
    ConsultaResumenResponse? UltimaConsulta,
    int? DietaValidadaId);

/// <summary>Historial clínico y de alimentación de un animal.</summary>
/// <param name="Animal">Resumen del animal.</param>
/// <param name="Consultas">Consultas veterinarias con sus tratamientos.</param>
/// <param name="Alertas">Alertas de salud del animal.</param>
/// <param name="Alimentacion">Registros de alimentación del animal.</param>
public sealed record HistorialAnimalResponse(
    AnimalResumenResponse Animal,
    IReadOnlyList<ConsultaDetalleResponse> Consultas,
    IReadOnlyList<AlertaResponse> Alertas,
    IReadOnlyList<RegistroAlimentacionResponse> Alimentacion);
