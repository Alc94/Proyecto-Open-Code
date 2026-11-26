using System.ComponentModel.DataAnnotations;
using ZooLogic.Api.Validation;

namespace ZooLogic.Api.Contracts.Animales;

/// <summary>Datos necesarios para crear un animal.</summary>
public sealed record CrearAnimalRequest
{
    /// <summary>Nombre propio del animal.</summary>
    [Required, MaxLength(60)]
    public required string Nombre { get; init; }

    /// <summary>Identificador de la especie a la que pertenece.</summary>
    [Range(1, int.MaxValue)]
    public required int EspecieId { get; init; }

    /// <summary>Sexo del animal: Macho o Hembra.</summary>
    [Required, ValorDeDominio(nameof(ValoresDominio.Sexos))]
    public required string Sexo { get; init; }

    /// <summary>Fecha de nacimiento; no puede ser futura.</summary>
    public required DateTimeOffset FechaNacimiento { get; init; }

    /// <summary>Peso actual en kilogramos.</summary>
    [Range(typeof(decimal), "0.001", "20000", ParseLimitsInInvariantCulture = true)]
    public required decimal PesoKg { get; init; }

    /// <summary>Recinto en el que vivirá el animal.</summary>
    [Required, MaxLength(60)]
    public required string Recinto { get; init; }

    /// <summary>Estado inicial; si se omite se usa Activo.</summary>
    [ValorDeDominio(nameof(ValoresDominio.EstadosAnimal))]
    public string? Estado { get; init; }

    /// <summary>URL de la fotografía del animal.</summary>
    [Url, MaxLength(300)]
    public string? FotoUrl { get; init; }
}

/// <summary>Datos para actualizar por completo un animal.</summary>
public sealed record ActualizarAnimalRequest
{
    /// <summary>Nombre propio del animal.</summary>
    [Required, MaxLength(60)]
    public required string Nombre { get; init; }

    /// <summary>Identificador de la especie a la que pertenece.</summary>
    [Range(1, int.MaxValue)]
    public required int EspecieId { get; init; }

    /// <summary>Sexo del animal: Macho o Hembra.</summary>
    [Required, ValorDeDominio(nameof(ValoresDominio.Sexos))]
    public required string Sexo { get; init; }

    /// <summary>Fecha de nacimiento; no puede ser futura.</summary>
    public required DateTimeOffset FechaNacimiento { get; init; }

    /// <summary>Peso actual en kilogramos.</summary>
    [Range(typeof(decimal), "0.001", "20000", ParseLimitsInInvariantCulture = true)]
    public required decimal PesoKg { get; init; }

    /// <summary>Recinto en el que vive el animal.</summary>
    [Required, MaxLength(60)]
    public required string Recinto { get; init; }

    /// <summary>Estado actual del animal.</summary>
    [Required, ValorDeDominio(nameof(ValoresDominio.EstadosAnimal))]
    public required string Estado { get; init; }

    /// <summary>URL de la fotografía del animal.</summary>
    [Url, MaxLength(300)]
    public string? FotoUrl { get; init; }
}
