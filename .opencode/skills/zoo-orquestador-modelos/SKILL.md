---
name: zoo-orquestador-modelos
description: Reparte el trabajo entre subagentes con modelos distintos (datos, backend, frontend, revisor) para ahorrar tokens. Cargar antes de cualquier tarea que toque varias capas del proyecto o que requiera investigar antes de programar.
---

# Orquestador de modelos

Cada subagente tiene su propio modelo y trabaja en una sesion hija con contexto limpio. Delegar bien ahorra tokens: el agente principal no arrastra documentacion ni codigo ajeno.

## A quien delegar

| Subagente | Cuando | Entrega |
|---|---|---|
| `datos` | Hay que recopilar informacion: documentacion de una libreria (Context7), ejemplos reales (Grep), esquema o contenido de la base de datos (postgres), datos semilla | Resumen corto con lo imprescindible |
| `backend` | Entidades, migraciones EF Core, servicios, controladores, simulacion de IA en la API | Archivos cambiados + resultado de `dotnet build` |
| `frontend` | Componentes, rutas, servicios HTTP, estilos Tailwind, chatbot | Archivos cambiados + resultado de `ng build` |
| `revisor` | Antes de dar por cerrada una funcionalidad | Lista de problemas por gravedad, sin editar |

Si la tarea es pequena (un cambio de una linea) hazla directamente: delegar tambien cuesta tokens.

## Como delegar (herramienta `subagent`)

1. Escribe un encargo **autocontenido**: objetivo, archivos implicados, restricciones y criterio de aceptacion. El subagente no ve la conversacion.
2. No pegues archivos enteros: indica rutas y deja que el subagente los lea.
3. Pide un informe de **maximo 150 palabras** y la lista de archivos tocados.
4. Orden habitual de una funcionalidad: `datos` (si hace falta investigar) -> `backend` -> `frontend` -> `revisor`.
5. Si el informe es insuficiente, haz una pregunta concreta de seguimiento en vez de repetir todo el encargo.

## Plantilla de encargo

```
Objetivo: <que debe existir al terminar>
Contexto: <modulo, entidades, endpoints implicados>
Archivos: <rutas a leer/modificar>
Restricciones: <skills a seguir (zoo-* y externas); sin secretos; IA simulada>
Aceptacion: <comando que debe pasar o comportamiento visible>
Informe: maximo 150 palabras + lista de archivos cambiados
```

## Registro para la presentacion

Al cerrar una tarea grande anade una linea a `docs/REGISTRO.md` con: fecha, tarea, subagentes usados y modelo de cada uno. Sirve como evidencia del ahorro y del reparto de modelos.
