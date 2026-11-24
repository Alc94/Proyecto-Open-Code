---
name: zoo-dominio
description: Vocabulario, entidades y reglas de negocio de un zoologico (animales, veterinaria y alimentacion). Cargar al disenar modelos de datos, endpoints, pantallas o datos de ejemplo del proyecto.
---

# Dominio del zoologico

Version actual: tres modulos. Si el proyecto crece se anadiran mas; no los inventes.

## Entidades principales

- **Especie**: `Id`, `NombreComun`, `NombreCientifico`, `Grupo` (Mamifero, Ave, Reptil, Anfibio, Pez), `TipoDieta` (Carnivoro, Herbivoro, Omnivoro), `EstadoConservacion`.
- **Animal**: `Id`, `Nombre`, `EspecieId`, `Sexo`, `FechaNacimiento`, `PesoKg`, `Recinto` (texto), `Estado` (Activo, EnObservacion, EnTratamiento, Cuarentena), `FotoUrl`.
- **ConsultaVeterinaria**: `Id`, `AnimalId`, `Fecha`, `Veterinario`, `Motivo`, `Diagnostico`, `PesoKg`, `Temperatura`.
- **Tratamiento**: `Id`, `ConsultaId`, `Medicamento`, `Dosis`, `FechaInicio`, `FechaFin`, `Notas`.
- **AlertaSalud**: `Id`, `AnimalId`, `Fecha`, `Gravedad` (Baja, Media, Alta, Critica), `Tipo`, `Mensaje`, `Estado` (Abierta, Revisada, Cerrada), `Origen` (siempre "IA simulada").
- **Alimento**: `Id`, `Nombre`, `Categoria`, `KcalPorKg`.
- **Dieta**: `Id`, `EspecieId`, `Nombre`, `Estado` (Propuesta, Validada, Descartada), `ValidadaPor`, `Origen`.
- **DietaLinea**: `DietaId`, `AlimentoId`, `CantidadKgDia`, `Frecuencia`.
- **RegistroAlimentacion**: `Id`, `AnimalId`, `Fecha`, `AlimentoId`, `CantidadKg`, `Consumido` (porcentaje).

## Reglas de negocio

1. Un animal pertenece a una especie; su dieta se basa en la de su especie.
2. Un tratamiento activo pone al animal en estado `EnTratamiento`.
3. Una dieta generada por la IA simulada nace en estado `Propuesta` y solo pasa a `Validada` cuando un veterinario la acepta. Nunca se aplica una dieta en `Propuesta`.
4. Las alertas de gravedad `Alta` o `Critica` aparecen destacadas en el panel.
5. No se borran consultas ni tratamientos: se cierran (historial clinico).

## Datos de ejemplo

- Todos **ficticios**: animales con nombre propio, pesos y fechas inventados. No son referencia veterinaria real y la interfaz debe indicarlo en una nota discreta.
- Semilla minima: 8 especies variadas, 20 animales, 30 consultas, 6 dietas, 40 registros de alimentacion, 8 alertas.
- La semilla es **idempotente**: se puede ejecutar al arrancar sin duplicar datos.

## Nomenclatura

Clases y propiedades en espanol sin tildes. Rutas de API en kebab-case: `/api/animales`, `/api/consultas`, `/api/dietas`, `/api/alertas`. Textos visibles en espanol con tildes.

## Pantallas de la aplicacion

1. **Panel**: resumen, alertas abiertas destacadas, animales en tratamiento.
2. **Animales**: listado con filtros y ficha con historial.
3. **Veterinaria**: consultas y tratamientos por animal.
4. **Alimentacion**: dietas, registros de comida y boton "Generar propuesta de dieta (IA simulada)".
5. **Alertas de salud**: lista ordenada por gravedad.
6. **Asistente**: chat multilingue para visitantes (es, en, fr) con selector de idioma.

Identidad visual de zoologico: paleta de verdes y tierras con un acento calido, tarjetas con fotografia de animal, diseno responsive (movil primero), contraste suficiente y estados de carga, vacio y error en cada listado.
