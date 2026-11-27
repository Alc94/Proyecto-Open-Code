# Zoo-Logic IA (proyecto con OpenCode)

Plataforma web de gestion de zoologicos (animales, veterinaria y alimentacion) con IA simulada, desarrollada con OpenCode usando skills, MCPs, providers y plugins. Un solo repositorio y un solo despliegue en Render.

## Elementos de OpenCode del proyecto

| Tipo | Elementos | Donde estan |
|---|---|---|
| Skills propias (3) | `zoo-dominio`, `zoo-orquestador-modelos`, `zoo-despliegue` | `.opencode/skills/` |
| Skills externas (4) | `angular-developer` (Angular), `dotnet-webapi` y `optimizing-ef-core-queries` (.NET), `frontend-design` (Anthropic) | se descargan de sus repositorios oficiales a `.opencode/skills/` |
| Agentes (4) | `datos`, `backend`, `frontend`, `revisor` | `.opencode/agents/` |
| MCPs (3) | Context7, Grep by Vercel, PostgreSQL | `opencode.jsonc` |
| Providers (4) | Principales: Google Gemini y OpenCode (modelos gratuitos). Respaldo manual: Groq. Tambien conectado: OpenRouter | se conectan con `/connect`; no estan en el repositorio |
| Plugins (2) | Dynamic Context Pruning (`.opencode/dcp.jsonc`), envsitter-guard | `opencode.jsonc` |

## Requisitos

Git y Git Bash, Docker Desktop, Node.js 24 o superior (Angular 21), SDK de .NET 10 y OpenCode v2 en su version de terminal (`opencode --version` debe responder).

## Puesta en marcha

1. Copia `.env.example` como `.env` y cambia la contrasena en las tres lineas (deja las comillas).
2. Levanta la base de datos: `docker compose up -d`.
3. Arranca OpenCode con las variables cargadas, desde Git Bash: `bash scripts/opencode.sh`.
4. Conecta los providers dentro de OpenCode con `/connect` y elige modelos con `/models`.
5. Comprueba MCPs y plugins: `opencode mcp list` y `opencode plugin list`.

## Comprobar la web en local

- **Desarrollo:** con PostgreSQL y la API en marcha (`bash scripts/opencode.sh` carga
  `ConnectionStrings__Default`), ejecuta `npm install && npm start` en `frontend/` y
  abre `http://localhost:4200`. El proxy de Angular envía `/api` al puerto 5142.
- **Producción local:** `npm run build` en `frontend/`, publica la API con
  `dotnet publish backend/src/ZooLogic.Api/ZooLogic.Api.csproj -c Release -o publicacion-local -p:UseAppHost=false`,
  copia `frontend/dist/frontend/browser` a `publicacion-local/wwwroot` y arranca con
  `PORT=5199 dotnet ZooLogic.Api.dll` desde la carpeta publicada. Abre
  `http://localhost:5199` y recarga `/animales/1` para comprobar el fallback del SPA.
  Los pasos detallados están en `frontend/README.md`.
- **Imagen de Render:** `docker build -t zoo-logic-ia .` y
  `docker run --rm -p 8080:8080 -e DATABASE_URL="postgresql://..." zoo-logic-ia`.

La API sirve la web compilada desde `wwwroot` (una sola URL) y responde `/health`.
La ruta `''` muestra la portada pública; `/panel` es el acceso del personal.

## Seguridad

`.env` no se sube a GitHub (esta en `.gitignore`) y el plugin envsitter-guard impide que el agente lo lea. Las claves de los providers las guarda OpenCode en tu carpeta de usuario, fuera del repositorio. Nunca pegues una clave en un archivo del proyecto ni en una captura.

## Aviso sobre la IA

Las alertas de salud, el asistente multilingue y las dietas generativas son una **simulacion** con datos ficticios. La interfaz lo indica y las dietas quedan pendientes de validacion veterinaria.
