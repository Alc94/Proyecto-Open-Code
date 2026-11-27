# Frontend de Zoo-Logic

Aplicación Angular 21 con Tailwind CSS 4: portada pública y área de gestión
(panel, animales, ficha de animal y alimentación).

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

Todas las peticiones usan rutas relativas que empiezan por `/api`. En desarrollo,
`proxy.conf.json` redirige `/api` a `http://localhost:5142`; en producción la propia
API sirve el frontend desde `wwwroot`, así que no hace falta CORS ni cambiar la URL.

## Compilación

```bash
npm run build
```

El resultado se genera en `dist/frontend/browser`. El `Dockerfile` de la raíz ya lo
copia al `wwwroot` de la API automáticamente.

## Comprobar la versión de producción en local (antes de Render)

Desde la raíz del repositorio, en Git Bash:

```bash
cd frontend && npm run build && cd ..
PUBLICAR="$PWD/publicacion-local"
dotnet publish backend/src/ZooLogic.Api/ZooLogic.Api.csproj -c Release -o "$PUBLICAR" -p:UseAppHost=false
mkdir -p "$PUBLICAR/wwwroot"
cp -r frontend/dist/frontend/browser/. "$PUBLICAR/wwwroot/"
cd "$PUBLICAR"
PORT=5199 ASPNETCORE_ENVIRONMENT=Production \
  ConnectionStrings__Default="Host=localhost;Port=5433;Database=zoologic;Username=zoo;Password=TU_CLAVE" \
  dotnet ZooLogic.Api.dll
```

Abre `http://localhost:5199` y recarga rutas como `/animales/1` o `/alimentacion`:
debemos ver la página, no un 404. Comprueba que `/health` responde y que
`/api/no-existe` devuelve JSON, no HTML. Sustituye `TU_CLAVE` por la de tu `.env`
(nunca la subas al repositorio).

También puedes probar la imagen exacta de Render:

```bash
docker build -t zoo-logic-ia .
docker run --rm -p 8080:8080 -e DATABASE_URL="postgresql://..." zoo-logic-ia
```
