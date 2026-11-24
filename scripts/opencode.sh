#!/usr/bin/env bash
# Lanza OpenCode cargando antes las variables de .env, para que el MCP de PostgreSQL
# vea DATABASE_URI y la API vea ConnectionStrings__Default.
# Las claves de los providers (Groq, Gemini, OpenRouter) NO van aqui: se guardan con /connect.
# Uso desde la raiz del proyecto, en Git Bash:  bash scripts/opencode.sh
set -euo pipefail
cd "$(dirname "$0")/.."
if [ -f .env ]; then
  set -a
  . ./.env
  set +a
else
  echo "Aviso: no existe .env; el MCP de PostgreSQL no podra conectar." >&2
fi
exec opencode "$@"
