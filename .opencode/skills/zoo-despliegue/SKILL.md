---
name: zoo-despliegue
description: Como conectar la API con PostgreSQL (local y Render), servir el frontend Angular desde wwwroot y preparar el Dockerfile y el render.yaml de Zoo-Logic. Cargar al configurar la base de datos, al unir backend y frontend o al desplegar en Render.
---

# Despliegue de Zoo-Logic (un servicio, una URL)

Esta skill solo recoge lo especifico del proyecto. Las convenciones generales de ASP.NET Core y de Angular las dan las skills `dotnet-webapi`, `optimizing-ef-core-queries` y `angular-developer`.

## Conexion a PostgreSQL

Leer la cadena en este orden: variable `DATABASE_URL` (Render) y, si no existe, la variable de entorno `ConnectionStrings__Default` (desarrollo; llega desde `.env`, nunca desde `appsettings.json`). Render entrega una URL `postgresql://usuario:clave@host:puerto/base`; Npgsql necesita formato clave=valor:

```csharp
static string ANpgsql(string url)
{
    var uri = new Uri(url);
    var info = uri.UserInfo.Split(':', 2);
    return $"Host={uri.Host};Port={(uri.Port > 0 ? uri.Port : 5432)};" +
           $"Database={uri.AbsolutePath.TrimStart('/')};" +
           $"Username={Uri.UnescapeDataString(info[0])};" +
           $"Password={Uri.UnescapeDataString(info[1])};SSL Mode=Prefer";
}
```

Si la conexion falla por SSL, prueba `SSL Mode=Require` o `Disable` segun el mensaje de error. Aplicar migraciones con `Database.Migrate()` al arrancar.

## Servir Angular desde la API

```csharp
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();            // o los Map<Recurso> de minimal APIs
app.MapGet("/health", () => Results.Ok(new { estado = "ok" }));
app.MapFallbackToFile("index.html");   // rutas del SPA
```

El frontend llama a la API con rutas **relativas** (`/api/animales`): en produccion comparten origen. CORS solo en desarrollo, cuando Angular corre con `ng serve`.

## Dockerfile y render.yaml

- `Dockerfile` multi-etapa en la raiz: compilar Angular (`npm ci` y `npm run build`), publicar la API (`dotnet publish`) y copiar `dist/<app>/browser` a `wwwroot` en la imagen final. El puerto sale de la variable `PORT` (`ASPNETCORE_HTTP_PORTS`).
- `render.yaml` en la raiz con un servicio web `runtime: docker`, `plan: free`, `healthCheckPath: /health` (siempre empieza por `/`), y una base de datos `plan: free`. La variable `DATABASE_URL` del servicio se enlaza con `fromDatabase` (`property: connectionString`), de modo que no hay credenciales en el repositorio.
- Sin `region` en ambos, los dos usan la misma por defecto.
- Plan gratuito: el servicio web se duerme tras 15 minutos sin visitas y la base de datos gratuita caduca a los 30 dias.
- Comprobar siempre con `docker build -t zoo-logic-ia .` antes de subir.
