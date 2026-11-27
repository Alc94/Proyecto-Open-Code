using System.ComponentModel.DataAnnotations;
using ZooLogic.Api.Validation;

namespace ZooLogic.Api.Contracts.Consultas;

/// <summary>Resumen de una consulta veterinaria.</summary>
/// <param name="Id">Identificador de la consulta.</param>
/// <param name="AnimalId">Identificador del animal atendido.</param>
/// <param name="AnimalNombre">Nombre del animal atendido.</param>
/// <param name="Fecha">Fecha y hora de la consulta.</param>
/// <param name="Veterinario">Veterinario que atendió la consulta.</param>
/// <param name="Motivo">Motivo de la consulta.</param>
/// <param name="Diagnostico">Diagnóstico registrado.</param>
public sealed record ConsultaResumenResponse(
    int Id,
    int AnimalId,
    string AnimalNombre,
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

/// <summary>Datos para crear un tratamiento dentro de una consulta.</summary>
public sealed record CrearTratamientoRequest
{
    /// <summary>Medicamento prescrito.</summary>
    [Required, MaxLength(100)]
    public required string Medicamento { get; init; }

    /// <summary>Dosis indicada.</summary>
    [Required, MaxLength(100)]
    public required string Dosis { get; init; }

    /// <summary>Fecha de inicio del tratamiento.</summary>
    public required DateTimeOffset FechaInicio { get; init; }

    /// <summary>Fecha de fin, si el tratamiento ya está cerrado.</summary>
    public DateTimeOffset? FechaFin { get; init; }

    /// <summary>Notas adicionales, si existen.</summary>
    [MaxLength(500)]
    public string? Notas { get; init; }
}

/// <summary>Datos para registrar una consulta veterinaria.</summary>
public sealed record CrearConsultaRequest
{
    /// <summary>Identificador del animal atendido.</summary>
    [Range(1, int.MaxValue)]
    public required int AnimalId { get; init; }

    /// <summary>Fecha y hora de la consulta.</summary>
    public required DateTimeOffset Fecha { get; init; }

    /// <summary>Veterinario que atendió la consulta.</summary>
    [Required, MaxLength(100)]
    public required string Veterinario { get; init; }

    /// <summary>Motivo de la consulta.</summary>
    [Required, MaxLength(200)]
    public required string Motivo { get; init; }

    /// <summary>Diagnóstico registrado.</summary>
    [Required, MaxLength(1000)]
    public required string Diagnostico { get; init; }

    /// <summary>Peso medido durante la consulta, si se registró.</summary>
    [Range(typeof(decimal), "0.001", "20000", ParseLimitsInInvariantCulture = true)]
    public decimal? PesoKg { get; init; }

    /// <summary>Temperatura medida durante la consulta, si se registró.</summary>
    [Range(typeof(decimal), "0", "60", ParseLimitsInInvariantCulture = true)]
    public decimal? Temperatura { get; init; }

    /// <summary>Tratamientos a crear junto con la consulta.</summary>
    public IReadOnlyList<CrearTratamientoRequest>? Tratamientos { get; init; }
}

/// <summary>Datos para cerrar un tratamiento activo.</summary>
public sealed record CerrarTratamientoRequest
{
    /// <summary>Fecha de fin; si se omite se usa la fecha y hora actual.</summary>
    public DateTimeOffset? FechaFin { get; init; }

    /// <summary>Notas finales del tratamiento.</summary>
    [MaxLength(500)]
    public string? Notas { get; init; }
}
