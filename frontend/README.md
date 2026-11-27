# Frontend de Zoo-Logic

Aplicación Angular 21 con Tailwind CSS 4 para el panel de gestión del zoo.

## Desarrollo

1. Arranca PostgreSQL con `docker compose up -d` desde la raíz del repositorio.
2. Arranca la API en el puerto 5142. Debe tener cargada la variable
   `ConnectionStrings__Default`; desde Git Bash, en la raíz del repositorio:

```bash
set -a
. ./.env
set +a
cd backend/src/ZooLogic.Api
dotnet run --launch-profile http
```

Si arrancas `dotnet run` en una terminal sin esas variables, la API se cierra al
inicio con «Falta la cadena de conexión de PostgreSQL» y el frontend mostrará
«No se pudo conectar con la API».

3. En esta carpeta, instala dependencias y arranca Angular:

```bash
npm install
npm start
```

La aplicación queda en `http://localhost:4200`. Comprueba la API antes de abrir la
web con `curl http://localhost:5142/health`, que debe devolver `{"status":"Healthy"}`.

## Llamadas a la API

Todas las peticiones usan rutas relativas que empiezan por `/api`. En desarrollo,
`proxy.conf.json` redirige `/api` a `http://localhost:5142`; en producción la propia
API sirve el frontend desde `wwwroot`, así que no hace falta CORS ni cambiar la URL.

## Compilación

```bash
npm run build
```

El resultado se genera en `dist/frontend/browser` y puede copiarse al `wwwroot` de la API.
