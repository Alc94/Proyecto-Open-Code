# Zoo-Logic IA (proyecto con OpenCode)

Plataforma web de gestion de zoologicos (animales, veterinaria y alimentacion) con IA simulada, desarrollada con OpenCode usando skills, MCPs, providers y plugins. Un solo repositorio y un solo despliegue en Render.

## Elementos de OpenCode del proyecto

| Tipo | Elementos | Donde estan |
|---|---|---|
| Skills propias (4) | `zoo-dominio`, `zoo-ia-features`, `zoo-orquestador-modelos`, `zoo-despliegue` | `.opencode/skills/` |
| Skills externas (4) | `angular-developer` (Angular), `dotnet-webapi` y `optimizing-ef-core-queries` (.NET), `frontend-design` (Anthropic) | se descargan de sus repositorios oficiales a `.opencode/skills/` |
| Agentes (4) | `datos`, `backend`, `frontend`, `revisor` | `.opencode/agents/` |
| MCPs (3) | Context7, Grep by Vercel, PostgreSQL | `opencode.jsonc` |
| Providers (4) | Principales: Google Gemini y OpenCode (modelos gratuitos). Respaldo manual: Groq. Tambien conectado: OpenRouter | se conectan con `/connect`; no estan en el repositorio |
| Plugins (2) | Dynamic Context Pruning (`.opencode/dcp.jsonc`), envsitter-guard | `opencode.jsonc` |

## Requisitos

Git y Git Bash, Docker Desktop, Node.js 22 o superior, SDK de .NET 10 y OpenCode v2 en su version de terminal (`opencode --version` debe responder).

## Puesta en marcha

1. Copia `.env.example` como `.env` y cambia la contrasena en las tres lineas (deja las comillas).
2. Levanta la base de datos: `docker compose up -d`.
3. Arranca OpenCode con las variables cargadas, desde Git Bash: `bash scripts/opencode.sh`.
4. Conecta los providers dentro de OpenCode con `/connect` y elige modelos con `/models`.
5. Comprueba MCPs y plugins: `opencode mcp list` y `opencode plugin list`.

## Seguridad

`.env` no se sube a GitHub (esta en `.gitignore`) y el plugin envsitter-guard impide que el agente lo lea. Las claves de los providers las guarda OpenCode en tu carpeta de usuario, fuera del repositorio. Nunca pegues una clave en un archivo del proyecto ni en una captura.

## Aviso sobre la IA

Las alertas de salud, el asistente multilingue y las dietas generativas son una **simulacion** con datos ficticios. La interfaz lo indica y las dietas quedan pendientes de validacion veterinaria.
