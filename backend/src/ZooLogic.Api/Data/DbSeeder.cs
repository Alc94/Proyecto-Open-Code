using Microsoft.EntityFrameworkCore;
using ZooLogic.Api.Models;

namespace ZooLogic.Api.Data;

/// <summary>
/// Carga los datos ficticios de demostración. Es idempotente: si ya hay especies no hace nada.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(ZooDbContext db, CancellationToken ct = default)
    {
        if (await db.Especies.AnyAsync(ct))
        {
            return;
        }

        var ahora = DateTimeOffset.UtcNow;
        DateTimeOffset Hace(int dias) => ahora.AddDays(-dias);

        await using var transaccion = await db.Database.BeginTransactionAsync(ct);

        // ---------- Especies ----------
        var especies = new List<Especie>
        {
            new() { NombreComun = "León africano", NombreCientifico = "Panthera leo", Grupo = "Mamifero", TipoDieta = "Carnivoro", EstadoConservacion = "Vulnerable" },
            new() { NombreComun = "Jirafa", NombreCientifico = "Giraffa camelopardalis", Grupo = "Mamifero", TipoDieta = "Herbivoro", EstadoConservacion = "Vulnerable" },
            new() { NombreComun = "Elefante africano", NombreCientifico = "Loxodonta africana", Grupo = "Mamifero", TipoDieta = "Herbivoro", EstadoConservacion = "En peligro" },
            new() { NombreComun = "Águila real", NombreCientifico = "Aquila chrysaetos", Grupo = "Ave", TipoDieta = "Carnivoro", EstadoConservacion = "Preocupación menor" },
            new() { NombreComun = "Flamenco común", NombreCientifico = "Phoenicopterus roseus", Grupo = "Ave", TipoDieta = "Omnivoro", EstadoConservacion = "Preocupación menor" },
            new() { NombreComun = "Tortuga gigante de Aldabra", NombreCientifico = "Aldabrachelys gigantea", Grupo = "Reptil", TipoDieta = "Herbivoro", EstadoConservacion = "Vulnerable" },
            new() { NombreComun = "Rana dardo azul", NombreCientifico = "Dendrobates tinctorius", Grupo = "Anfibio", TipoDieta = "Carnivoro", EstadoConservacion = "Preocupación menor" },
            new() { NombreComun = "Pez payaso", NombreCientifico = "Amphiprion ocellaris", Grupo = "Pez", TipoDieta = "Omnivoro", EstadoConservacion = "Preocupación menor" }
        };

        db.Especies.AddRange(especies);
        await db.SaveChangesAsync(ct);

        var leon = especies[0];
        var jirafa = especies[1];
        var elefante = especies[2];
        var aguila = especies[3];
        var flamenco = especies[4];
        var tortuga = especies[5];
        var rana = especies[6];
        var pez = especies[7];

        // ---------- Animales ----------
        var animales = new List<Animal>
        {
            new() { Nombre = "Simba", EspecieId = leon.Id, Sexo = "Macho", FechaNacimiento = Hace(3650), PesoKg = 192.40m, Recinto = "Sabana norte", Estado = "EnTratamiento" },
            new() { Nombre = "Nala", EspecieId = leon.Id, Sexo = "Hembra", FechaNacimiento = Hace(3280), PesoKg = 131.20m, Recinto = "Sabana norte", Estado = "Activo" },
            new() { Nombre = "Melman", EspecieId = jirafa.Id, Sexo = "Macho", FechaNacimiento = Hace(4200), PesoKg = 1080.00m, Recinto = "Sabana sur", Estado = "Activo" },
            new() { Nombre = "Kira", EspecieId = jirafa.Id, Sexo = "Hembra", FechaNacimiento = Hace(2900), PesoKg = 860.50m, Recinto = "Sabana sur", Estado = "Activo" },
            new() { Nombre = "Dumbo", EspecieId = elefante.Id, Sexo = "Macho", FechaNacimiento = Hace(9100), PesoKg = 5400.00m, Recinto = "Sabana este", Estado = "EnObservacion" },
            new() { Nombre = "Tembo", EspecieId = elefante.Id, Sexo = "Macho", FechaNacimiento = Hace(7800), PesoKg = 4820.00m, Recinto = "Sabana este", Estado = "EnTratamiento" },
            new() { Nombre = "Aquila", EspecieId = aguila.Id, Sexo = "Hembra", FechaNacimiento = Hace(2400), PesoKg = 5.20m, Recinto = "Aviario grande", Estado = "Activo" },
            new() { Nombre = "Tormenta", EspecieId = aguila.Id, Sexo = "Macho", FechaNacimiento = Hace(1800), PesoKg = 4.40m, Recinto = "Aviario grande", Estado = "Activo" },
            new() { Nombre = "Rosa", EspecieId = flamenco.Id, Sexo = "Hembra", FechaNacimiento = Hace(1200), PesoKg = 3.10m, Recinto = "Laguna", Estado = "EnTratamiento" },
            new() { Nombre = "Coral", EspecieId = flamenco.Id, Sexo = "Hembra", FechaNacimiento = Hace(980), PesoKg = 2.90m, Recinto = "Laguna", Estado = "Activo" },
            new() { Nombre = "Pluma", EspecieId = flamenco.Id, Sexo = "Macho", FechaNacimiento = Hace(760), PesoKg = 2.60m, Recinto = "Laguna", Estado = "Cuarentena" },
            new() { Nombre = "Shelly", EspecieId = tortuga.Id, Sexo = "Hembra", FechaNacimiento = Hace(14600), PesoKg = 210.00m, Recinto = "Terrario exterior", Estado = "EnTratamiento" },
            new() { Nombre = "Tuga", EspecieId = tortuga.Id, Sexo = "Macho", FechaNacimiento = Hace(12800), PesoKg = 186.50m, Recinto = "Terrario exterior", Estado = "Activo" },
            new() { Nombre = "Mafalda", EspecieId = tortuga.Id, Sexo = "Hembra", FechaNacimiento = Hace(9400), PesoKg = 152.30m, Recinto = "Terrario exterior", Estado = "Activo" },
            new() { Nombre = "Azul", EspecieId = rana.Id, Sexo = "Macho", FechaNacimiento = Hace(420), PesoKg = 0.03m, Recinto = "Herpetario 1", Estado = "Activo" },
            new() { Nombre = "Zafiro", EspecieId = rana.Id, Sexo = "Macho", FechaNacimiento = Hace(380), PesoKg = 0.03m, Recinto = "Herpetario 1", Estado = "Activo" },
            new() { Nombre = "Turquesa", EspecieId = rana.Id, Sexo = "Hembra", FechaNacimiento = Hace(300), PesoKg = 0.02m, Recinto = "Herpetario 2", Estado = "Activo" },
            new() { Nombre = "Nemo", EspecieId = pez.Id, Sexo = "Macho", FechaNacimiento = Hace(540), PesoKg = 0.01m, Recinto = "Acuario tropical", Estado = "Activo" },
            new() { Nombre = "Burbuja", EspecieId = pez.Id, Sexo = "Hembra", FechaNacimiento = Hace(500), PesoKg = 0.01m, Recinto = "Acuario tropical", Estado = "Activo" },
            new() { Nombre = "Punto", EspecieId = pez.Id, Sexo = "Macho", FechaNacimiento = Hace(460), PesoKg = 0.01m, Recinto = "Acuario tropical", Estado = "Activo" }
        };

        db.Animales.AddRange(animales);
        await db.SaveChangesAsync(ct);

        var grupoPorEspecie = especies.ToDictionary(x => x.Id, x => x.Grupo);
        var dietaPorEspecie = especies.ToDictionary(x => x.Id, x => x.TipoDieta);

        // ---------- Consultas veterinarias ----------
        var veterinarios = new[]
        {
            "Dra. Elena Ruiz", "Dr. Marcos Vidal", "Dra. Paula Serrano", "Dr. Iván Castro"
        };

        var motivos = new[]
        {
            "Revisión anual", "Cojera", "Pérdida de apetito", "Herida superficial", "Control de peso",
            "Tos persistente", "Picor en la piel", "Revisión dental", "Diarrea", "Control postoperatorio"
        };

        var diagnosticos = new[]
        {
            "Sin hallazgos relevantes", "Cojera leve en miembro posterior", "Deshidratación leve",
            "Herida superficial en flanco", "Sobrepeso moderado", "Dermatitis bacteriana",
            "Cuadro respiratorio leve", "Anemia leve", "Absceso dental", "Parasitosis gastrointestinal"
        };

        var consultas = new List<ConsultaVeterinaria>();
        for (var i = 0; i < 30; i++)
        {
            var animal = animales[i % animales.Count];
            var grupo = grupoPorEspecie[animal.EspecieId];
            var factorPeso = 1m + ((i % 5) - 2) * 0.01m;

            consultas.Add(new ConsultaVeterinaria
            {
                AnimalId = animal.Id,
                Fecha = ahora.AddDays(-(15 + i * 16)),
                Veterinario = veterinarios[i % veterinarios.Length],
                Motivo = motivos[i % motivos.Length],
                Diagnostico = diagnosticos[i % diagnosticos.Length],
                PesoKg = Math.Round(animal.PesoKg * factorPeso, 2),
                Temperatura = TemperaturaPara(grupo, i)
            });
        }

        db.Consultas.AddRange(consultas);
        await db.SaveChangesAsync(ct);

        // ---------- Tratamientos ----------
        var tratamientos = new List<Tratamiento>
        {
            new() { ConsultaId = consultas[0].Id, Medicamento = "Meloxicam", Dosis = "0,2 mg/kg cada 24 h", FechaInicio = Hace(10), FechaFin = null, Notas = "Tratamiento activo por cojera leve." },
            new() { ConsultaId = consultas[3].Id, Medicamento = "Amoxicilina", Dosis = "10 mg/kg cada 12 h", FechaInicio = Hace(64), FechaFin = Hace(54), Notas = "Pauta completada sin incidencias." },
            new() { ConsultaId = consultas[5].Id, Medicamento = "Ivermectina", Dosis = "0,2 mg/kg dosis única", FechaInicio = Hace(8), FechaFin = null, Notas = "Control de parásitos externos." },
            new() { ConsultaId = consultas[8].Id, Medicamento = "Enrofloxacina", Dosis = "5 mg/kg cada 24 h", FechaInicio = Hace(5), FechaFin = null, Notas = "Cuadro respiratorio leve." },
            new() { ConsultaId = consultas[11].Id, Medicamento = "Calcio oral", Dosis = "1 ml/kg cada 48 h", FechaInicio = Hace(20), FechaFin = null, Notas = "Refuerzo de calcio por caparazón blando." },
            new() { ConsultaId = consultas[14].Id, Medicamento = "Metronidazol", Dosis = "10 mg/kg cada 24 h", FechaInicio = Hace(60), FechaFin = Hace(53), Notas = "Tratamiento digestivo completado." },
            new() { ConsultaId = consultas[17].Id, Medicamento = "Sal marina", Dosis = "1 g/L en el agua", FechaInicio = Hace(30), FechaFin = Hace(23), Notas = "Ajuste osmótico del acuario." },
            new() { ConsultaId = consultas[23].Id, Medicamento = "Vitamina B12", Dosis = "1 ml IM dosis única", FechaInicio = Hace(90), FechaFin = Hace(90), Notas = "Refuerzo tras analítica." },
            new() { ConsultaId = consultas[26].Id, Medicamento = "Ketoprofeno", Dosis = "1 mg/kg cada 24 h", FechaInicio = Hace(120), FechaFin = Hace(113), Notas = "Antiinflamatorio por traumatismo leve." },
            new() { ConsultaId = consultas[29].Id, Medicamento = "Prazicuantel", Dosis = "5 mg/kg dosis única", FechaInicio = Hace(150), FechaFin = Hace(150), Notas = "Desparasitación interna." }
        };

        db.Tratamientos.AddRange(tratamientos);
        await db.SaveChangesAsync(ct);

        // ---------- Alimentos ----------
        var alimentos = new List<Alimento>
        {
            new() { Nombre = "Carne de res", Categoria = "Carnes", KcalPorKg = 2500.00m },
            new() { Nombre = "Hígado de pollo", Categoria = "Carnes", KcalPorKg = 1350.00m },
            new() { Nombre = "Pollo entero", Categoria = "Carnes", KcalPorKg = 2150.00m },
            new() { Nombre = "Roedores", Categoria = "Carnes", KcalPorKg = 1800.00m },
            new() { Nombre = "Heno de alfalfa", Categoria = "Forraje", KcalPorKg = 1800.00m },
            new() { Nombre = "Ramas de acacia", Categoria = "Forraje", KcalPorKg = 1200.00m },
            new() { Nombre = "Zanahoria", Categoria = "Vegetales", KcalPorKg = 410.00m },
            new() { Nombre = "Verduras de hoja", Categoria = "Vegetales", KcalPorKg = 250.00m },
            new() { Nombre = "Fruta variada", Categoria = "Frutas", KcalPorKg = 520.00m },
            new() { Nombre = "Invertebrados varios", Categoria = "Invertebrados", KcalPorKg = 990.00m },
            new() { Nombre = "Pienso para flamencos", Categoria = "Pienso", KcalPorKg = 2800.00m },
            new() { Nombre = "Pienso para peces", Categoria = "Pienso", KcalPorKg = 3200.00m }
        };

        db.Alimentos.AddRange(alimentos);
        await db.SaveChangesAsync(ct);

        // ---------- Dietas ----------
        var dietas = new List<Dieta>
        {
            new() { EspecieId = leon.Id, Nombre = "Dieta carnívora base", Estado = "Validada", ValidadaPor = "Dra. Elena Ruiz", Origen = "Manual" },
            new() { EspecieId = jirafa.Id, Nombre = "Dieta de ramas y heno", Estado = "Validada", ValidadaPor = "Dr. Marcos Vidal", Origen = "Manual" },
            new() { EspecieId = elefante.Id, Nombre = "Dieta herbívora de gran volumen", Estado = "Validada", ValidadaPor = "Dra. Paula Serrano", Origen = "Manual" },
            new() { EspecieId = aguila.Id, Nombre = "Dieta de ave rapaz", Estado = "Validada", ValidadaPor = "Dr. Iván Castro", Origen = "Manual" },
            new() { EspecieId = flamenco.Id, Nombre = "Propuesta omnívora para flamencos", Estado = "Propuesta", ValidadaPor = null, Origen = "IA simulada" },
            new() { EspecieId = tortuga.Id, Nombre = "Propuesta herbívora para quelonios", Estado = "Propuesta", ValidadaPor = null, Origen = "IA simulada" }
        };

        db.Dietas.AddRange(dietas);
        await db.SaveChangesAsync(ct);

        var lineas = new List<DietaLinea>
        {
            // León
            new() { DietaId = dietas[0].Id, AlimentoId = alimentos[0].Id, CantidadKgDia = 4.500m, Frecuencia = "Diario" },
            new() { DietaId = dietas[0].Id, AlimentoId = alimentos[1].Id, CantidadKgDia = 0.500m, Frecuencia = "3 veces por semana" },
            new() { DietaId = dietas[0].Id, AlimentoId = alimentos[2].Id, CantidadKgDia = 1.000m, Frecuencia = "2 veces por semana" },
            // Jirafa
            new() { DietaId = dietas[1].Id, AlimentoId = alimentos[4].Id, CantidadKgDia = 8.000m, Frecuencia = "Diario" },
            new() { DietaId = dietas[1].Id, AlimentoId = alimentos[5].Id, CantidadKgDia = 6.000m, Frecuencia = "Diario" },
            new() { DietaId = dietas[1].Id, AlimentoId = alimentos[6].Id, CantidadKgDia = 2.000m, Frecuencia = "Diario" },
            // Elefante
            new() { DietaId = dietas[2].Id, AlimentoId = alimentos[4].Id, CantidadKgDia = 40.000m, Frecuencia = "Diario" },
            new() { DietaId = dietas[2].Id, AlimentoId = alimentos[5].Id, CantidadKgDia = 15.000m, Frecuencia = "Diario" },
            new() { DietaId = dietas[2].Id, AlimentoId = alimentos[8].Id, CantidadKgDia = 10.000m, Frecuencia = "Diario" },
            // Águila
            new() { DietaId = dietas[3].Id, AlimentoId = alimentos[2].Id, CantidadKgDia = 0.800m, Frecuencia = "Diario" },
            new() { DietaId = dietas[3].Id, AlimentoId = alimentos[3].Id, CantidadKgDia = 0.300m, Frecuencia = "3 veces por semana" },
            // Flamenco (propuesta IA simulada)
            new() { DietaId = dietas[4].Id, AlimentoId = alimentos[10].Id, CantidadKgDia = 0.400m, Frecuencia = "Diario" },
            new() { DietaId = dietas[4].Id, AlimentoId = alimentos[9].Id, CantidadKgDia = 0.200m, Frecuencia = "Diario" },
            new() { DietaId = dietas[4].Id, AlimentoId = alimentos[7].Id, CantidadKgDia = 0.100m, Frecuencia = "Diario" },
            // Tortuga (propuesta IA simulada)
            new() { DietaId = dietas[5].Id, AlimentoId = alimentos[7].Id, CantidadKgDia = 2.000m, Frecuencia = "Diario" },
            new() { DietaId = dietas[5].Id, AlimentoId = alimentos[8].Id, CantidadKgDia = 1.000m, Frecuencia = "3 veces por semana" },
            new() { DietaId = dietas[5].Id, AlimentoId = alimentos[4].Id, CantidadKgDia = 1.500m, Frecuencia = "Diario" }
        };

        db.DietaLineas.AddRange(lineas);
        await db.SaveChangesAsync(ct);

        // ---------- Registros de alimentación ----------
        var consumos = new[] { 96m, 88m, 74m, 92m, 58m, 100m, 81m, 64m, 90m, 70m };
        var registros = new List<RegistroAlimentacion>();

        for (var i = 0; i < 40; i++)
        {
            var animal = animales[i % animales.Count];
            var grupo = grupoPorEspecie[animal.EspecieId];
            var tipoDieta = dietaPorEspecie[animal.EspecieId];
            var alimento = ElegirAlimento(alimentos, grupo, tipoDieta, i);
            var factor = FactorRacion(grupo, tipoDieta);

            registros.Add(new RegistroAlimentacion
            {
                AnimalId = animal.Id,
                Fecha = ahora.AddDays(-(1 + i % 18)).AddHours(-(i % 6)),
                AlimentoId = alimento.Id,
                CantidadKg = Math.Round(Math.Max(0.01m, animal.PesoKg * factor), 3),
                Consumido = consumos[i % consumos.Length]
            });
        }

        db.RegistrosAlimentacion.AddRange(registros);
        await db.SaveChangesAsync(ct);

        // ---------- Alertas de salud ----------
        var alertas = new List<AlertaSalud>
        {
            new() { AnimalId = animales[0].Id, Fecha = Hace(2), Gravedad = "Critica", Tipo = "Peso", Mensaje = "El peso ha bajado un 9 % en 14 días (192,4 kg a 175,1 kg).", Estado = "Abierta", Origen = "IA simulada" },
            new() { AnimalId = animales[5].Id, Fecha = Hace(1), Gravedad = "Alta", Tipo = "Consumo", Mensaje = "Consumo medio del 58 % en los últimos 3 registros de alimentación.", Estado = "Abierta", Origen = "IA simulada" },
            new() { AnimalId = animales[8].Id, Fecha = Hace(3), Gravedad = "Alta", Tipo = "Temperatura", Mensaje = "Temperatura de 41,8 °C registrada en la última consulta.", Estado = "Abierta", Origen = "IA simulada" },
            new() { AnimalId = animales[11].Id, Fecha = Hace(4), Gravedad = "Media", Tipo = "Tratamiento", Mensaje = "Tratamiento de calcio sin seguimiento veterinario en los últimos 7 días.", Estado = "Abierta", Origen = "IA simulada" },
            new() { AnimalId = animales[4].Id, Fecha = Hace(2), Gravedad = "Alta", Tipo = "Consumo", Mensaje = "Consumo medio del 64 % en los últimos 4 registros de alimentación.", Estado = "Abierta", Origen = "IA simulada" },
            new() { AnimalId = animales[10].Id, Fecha = Hace(10), Gravedad = "Media", Tipo = "Peso", Mensaje = "Variación de peso del -6 % respecto a la media de la especie.", Estado = "Revisada", Origen = "IA simulada" },
            new() { AnimalId = animales[14].Id, Fecha = Hace(20), Gravedad = "Baja", Tipo = "Consumo", Mensaje = "Consumo del 70 % en la última toma; conviene vigilar la siguiente.", Estado = "Cerrada", Origen = "IA simulada" },
            new() { AnimalId = animales[17].Id, Fecha = Hace(25), Gravedad = "Baja", Tipo = "Temperatura", Mensaje = "Temperatura del agua de 26,4 °C, ligeramente alta para la especie.", Estado = "Cerrada", Origen = "IA simulada" }
        };

        db.AlertasSalud.AddRange(alertas);
        await db.SaveChangesAsync(ct);

        await transaccion.CommitAsync(ct);
    }

    private static decimal TemperaturaPara(string grupo, int indice) => grupo switch
    {
        "Mamifero" => 36.5m + indice % 5 * 0.4m,
        "Ave" => 40.0m + indice % 4 * 0.5m,
        "Reptil" => 26.0m + indice % 6 * 1.0m,
        "Anfibio" => 20.0m + indice % 4 * 1.0m,
        "Pez" => 24.0m + indice % 3 * 1.0m,
        _ => 37.0m
    };

    private static Alimento ElegirAlimento(List<Alimento> alimentos, string grupo, string tipoDieta, int indice)
    {
        if (grupo == "Mamifero" && tipoDieta == "Carnivoro")
        {
            return alimentos[indice % 2 == 0 ? 0 : 2];
        }

        if (grupo == "Mamifero")
        {
            return alimentos[indice % 2 == 0 ? 4 : 6];
        }

        if (grupo == "Ave" && tipoDieta == "Carnivoro")
        {
            return alimentos[indice % 2 == 0 ? 2 : 3];
        }

        if (grupo == "Ave")
        {
            return alimentos[indice % 2 == 0 ? 10 : 9];
        }

        if (grupo == "Reptil")
        {
            return alimentos[indice % 2 == 0 ? 7 : 8];
        }

        if (grupo == "Anfibio")
        {
            return alimentos[9];
        }

        return alimentos[11];
    }

    private static decimal FactorRacion(string grupo, string tipoDieta) => grupo switch
    {
        "Mamifero" => tipoDieta == "Carnivoro" ? 0.025m : 0.03m,
        "Ave" => 0.08m,
        "Reptil" => 0.01m,
        "Anfibio" => 0.5m,
        _ => 0.05m
    };
}
