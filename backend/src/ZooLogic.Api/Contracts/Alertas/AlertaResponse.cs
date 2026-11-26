namespace ZooLogic.Api.Contracts.Alertas;

/// <summary>Alerta de salud de un animal.</summary>
/// <param name="Id">Identificador de la alerta.</param>
/// <param name="AnimalId">Identificador del animal afectado.</param>
/// <param name="AnimalNombre">Nombre del animal afectado.</param>
/// <param name="EspecieNombreComun">Nombre común de la especie.</param>
/// <param name="Fecha">Fecha en la que se generó la alerta.</param>
/// <param name="Gravedad">Gravedad: Baja, Media, Alta o Crítica.</param>
/// <param name="Tipo">Tipo de alerta.</param>
/// <param name="Mensaje">Explicación de la alerta.</param>
/// <param name="Estado">Estado: Abierta, Revisada o Cerrada.</param>
/// <param name="Origen">Origen de la alerta; siempre IA simulada.</param>
/// <param name="Destacada">Indica si debe mostrarse destacada en el panel.</param>
/// <param name="Confianza">Confianza ficticia de la simulación, si aplica.</param>
public sealed record AlertaResponse(
    int Id,
    int AnimalId,
    string AnimalNombre,
    string EspecieNombreComun,
    DateTimeOffset Fecha,
    string Gravedad,
    string Tipo,
    string Mensaje,
    string Estado,
    string Origen,
    bool Destacada,
    decimal? Confianza);
