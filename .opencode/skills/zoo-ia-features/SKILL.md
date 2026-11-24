---
name: zoo-ia-features
description: Como simular las funciones de IA del zoologico (alertas de salud, chatbot multilingue, dietas generativas) sin llamar a ningun modelo real. Cargar al implementar o modificar cualquiera de esas tres funciones.
---

# IA simulada

La aplicacion **no usa IA real**: se simula con logica determinista para demostrar el producto. Regla de honestidad: la simulacion siempre se etiqueta como tal en la interfaz y en el campo `Origen` de los datos ("IA simulada"). Nunca se presenta como un modelo entrenado.

## Arquitectura

Define una interfaz por funcion y una implementacion simulada, de modo que en el futuro pueda sustituirse por un proveedor real sin tocar controladores ni frontend:

```csharp
public interface IMotorAlertas   { IEnumerable<AlertaSalud> Analizar(Animal a, IEnumerable<ConsultaVeterinaria> historial); }
public interface IAsistente      { RespuestaAsistente Responder(string mensaje, string idioma); }
public interface IGeneradorDietas{ Dieta Proponer(Especie e, Animal? a); }
// Implementaciones: MotorAlertasSimulado, AsistenteSimulado, GeneradorDietasSimulado
```

Sin llaves API en el codigo ni en el frontend: no hay nada que filtrar.

## 1. Alertas de salud

Reglas sobre el historial: variacion de peso superior al umbral entre consultas, bajo consumo en varios registros de alimentacion consecutivos, temperatura fuera de rango, tratamiento sin seguimiento. Cada alerta incluye gravedad, mensaje explicativo ("El peso ha bajado un 9 % en 14 dias") y una "confianza" ficticia coherente con la regla. Se evalua al consultar el panel o con un endpoint `POST /api/alertas/analizar`.

## 2. Asistente multilingue para visitantes

- Idiomas: es, en, fr. Los textos viven en ficheros por idioma, no en el codigo.
- Deteccion de intencion por palabras clave: horarios, entradas, ubicacion de animales, alimentacion, normas, accesibilidad, contacto. Respuesta por defecto amable cuando no hay coincidencia.
- Pequeno retardo aleatorio y efecto de escritura para parecer natural; el aviso "Asistente de demostracion (IA simulada)" permanece visible.
- Los horarios y precios de las respuestas son datos de ejemplo.

## 3. Dietas generativas

El generador combina reglas por `TipoDieta` y `Grupo` de la especie con una pequena variacion (semilla por animal) para que cada propuesta parezca distinta. Devuelve una `Dieta` en estado `Propuesta` con lineas de alimento y una explicacion. La interfaz muestra "Pendiente de validacion veterinaria" y un boton para que el veterinario la valide o descarte. Los valores son de ejemplo: no deben presentarse como pautas reales de nutricion animal.

## Prueba rapida

Para cada funcion, un test que demuestre que, con la misma entrada, la salida es reproducible y que la etiqueta de simulacion esta presente.
