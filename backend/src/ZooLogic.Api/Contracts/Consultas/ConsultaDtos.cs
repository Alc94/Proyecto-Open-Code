namespace ZooLogic.Api.Contracts.Consultas;

/// <summary>Resumen de una consulta veterinaria.</summary>
/// <param name="Id">Identificador de la consulta.</param>
/// <param name="Fecha">Fecha y hora de la consulta.</param>
/// <param name="Veterinario">Veterinario que atendió la consulta.</param>
/// <param name="Motivo">Motivo de la consulta.</param>
/// <param name="Diagnostico">Diagnóstico registrado.</param>
public sealed record ConsultaResumenResponse(
    int Id,
    DateTimeOffset Fecha,
    string Veterinario,
    string Motivo,
    string Diagnostico);

/// <summary>Consulta veterinaria completa con sus tratamientos.</summary>
/// <param name="Id">Identificador de la consulta.</param>
/// <param name="AnimalId">Identificador del animal atendido.</param>
/// <param name="AnimalNombre">Nombre del animal atendido.</param>
/// <param name="Fecha">Fecha y hora de la consulta.</param>
/// <param name="Veterinario">Veterinario que atendió la consulta.</param>
/// <param name="Motivo">Motivo de la consulta.</param>
/// <param name="Diagnostico">Diagnóstico registrado.</param>
/// <param name="PesoKg">Peso registrado durante la consulta, si se midió.</param>
/// <param name="Temperatura">Temperatura registrada durante la consulta, si se midió.</param>
/// <param name="Tratamientos">Tratamientos asociados a la consulta.</param>
public sealed record ConsultaDetalleResponse(
    int Id,
    int AnimalId,
    string AnimalNombre,
    DateTimeOffset Fecha,
    string Veterinario,
    string Motivo,
    string Diagnostico,
    decimal? PesoKg,
    decimal? Temperatura,
    IReadOnlyList<TratamientoResponse> Tratamientos);

/// <summary>Tratamiento veterinario asociado a una consulta.</summary>
/// <param name="Id">Identificador del tratamiento.</param>
/// <param name="ConsultaId">Identificador de la consulta de origen.</param>
/// <param name="AnimalId">Identificador del animal tratado.</param>
/// <param name="AnimalNombre">Nombre del animal tratado.</param>
/// <param name="Medicamento">Medicamento prescrito.</param>
/// <param name="Dosis">Dosis indicada.</param>
/// <param name="FechaInicio">Fecha de inicio del tratamiento.</param>
/// <param name="FechaFin">Fecha de fin; null mientras sigue activo.</param>
/// <param name="Notas">Notas adicionales, si existen.</param>
/// <param name="Activo">Indica si el tratamiento sigue activo.</param>
public sealed record TratamientoResponse(
    int Id,
    int ConsultaId,
    int AnimalId,
    string AnimalNombre,
    string Medicamento,
    string Dosis,
    DateTimeOffset FechaInicio,
    DateTimeOffset? FechaFin,
    string? Notas,
    bool Activo);
