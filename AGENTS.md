# Zoo-Logic IA

Plataforma web de gestion de zoologicos. Modulos de esta version: **animales, veterinaria y alimentacion**. La IA de la aplicacion es **simulada** (ver skill `zoo-ia-features`): la web no llama a ningun modelo real.

## Estructura (monorepo, un solo despliegue)

```
backend/src/ZooLogic.Api/   ASP.NET Core Web API (.NET 10) + EF Core + PostgreSQL
frontend/                   Angular + Tailwind CSS
docker-compose.yml          PostgreSQL local de desarrollo (puerto 5433)
```

La API sirve el frontend compilado desde `wwwroot`: una sola URL y un solo servicio. El `Dockerfile` y el `render.yaml` se crean al desplegar.

## Reglas

- Idioma: interfaz, comentarios y mensajes de commit en espanol. Codigo con nombres de dominio en espanol sin tildes (`Animal`, `Especie`, `PesoKg`).
- Nunca leas, escribas ni muestres secretos (`.env`, claves API, contrasenas). Usa solo `.env.example`.
- La cadena de conexion de desarrollo llega por la variable de entorno `ConnectionStrings__Default`; nunca la escribas en `appsettings*.json`.
- Antes de usar una API de Angular, .NET o EF Core, consulta Context7 para no usar APIs obsoletas.
- Para inspeccionar la base de datos usa el MCP `postgres` (solo lectura). Los cambios de esquema van siempre en migraciones de EF Core.
- La API debe exponer `GET /health` y leer el puerto de la variable `PORT`.
- Cada cambio relevante termina con compilacion correcta (`dotnet build`, `npm run build`).

## Skills

Propias del proyecto: `zoo-dominio`, `zoo-ia-features`, `zoo-orquestador-modelos`, `zoo-despliegue`. Externas oficiales: `angular-developer` (Angular), `dotnet-webapi` y `optimizing-ef-core-queries` (.NET) y `frontend-design` (interfaz). Si una externa no esta instalada, trabaja con Context7 y las skills propias.

## Reparto de trabajo entre modelos

Carga la skill `zoo-orquestador-modelos` antes de tareas grandes. Resumen: `datos` investiga, `backend` programa la API, `frontend` programa Angular, `revisor` revisa. Cada subagente recibe un encargo autocontenido y devuelve un informe corto.

Providers: los principales son Google Gemini y OpenCode (solo modelos gratuitos). Groq es el respaldo y se usa de forma manual: si un modelo falla por limite de uso o indisponibilidad, cambia al siguiente modelo gratuito con `/models` o cambia la linea `model:` del agente. Nunca uses modelos de pago.
