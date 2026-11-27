using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ZooLogic.Api.Data;
using ZooLogic.Api.Endpoints;
using ZooLogic.Api.Middleware;
using ZooLogic.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// En Render el puerto llega por la variable PORT.
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

// En Render la cadena llega como URL en DATABASE_URL; en desarrollo se usa ConnectionStrings__Default.
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
var connectionString = !string.IsNullOrWhiteSpace(databaseUrl)
    ? ANpgsql(databaseUrl)
    : builder.Configuration.GetConnectionString("Default");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Falta la cadena de conexión de PostgreSQL. Define DATABASE_URL en Render o ConnectionStrings__Default en desarrollo.");
}

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddValidation();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();

// El frontend Angular de desarrollo se sirve desde el puerto 4200.
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDev", policy =>
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<ZooDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IEspecieService, EspecieService>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();
builder.Services.AddScoped<ITratamientoService, TratamientoService>();
builder.Services.AddScoped<IAlertaService, AlertaService>();
builder.Services.AddScoped<IAlimentoService, AlimentoService>();
builder.Services.AddScoped<IDietaService, DietaService>();
builder.Services.AddScoped<IRegistroAlimentacionService, RegistroAlimentacionService>();
builder.Services.AddScoped<IPanelService, PanelService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Preparar la base de datos también en Render; la API arranca aunque falle y /health responderá 503.
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ZooDbContext>();
        await db.Database.MigrateAsync();
        await DbSeeder.SeedAsync(db);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "No se pudo preparar la base de datos al arrancar. /health responderá Unhealthy.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseCors("FrontendDev");
}

app.UseExceptionHandler();
app.UseStatusCodePages();

// El frontend compilado se sirve desde wwwroot (Docker copia allí dist/frontend/browser).
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/health", async (ZooDbContext db, CancellationToken ct) =>
    {
        bool healthy;
        try
        {
            healthy = await db.Database.CanConnectAsync(ct);
        }
        catch
        {
            healthy = false;
        }

        return healthy
            ? Results.Ok(new { status = "Healthy" })
            : Results.Json(new { status = "Unhealthy" }, statusCode: StatusCodes.Status503ServiceUnavailable);
    })
    .WithName("GetHealth")
    .WithSummary("Comprueba el estado de la API y de la base de datos.")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status503ServiceUnavailable);

app.MapEspecies();
app.MapAnimales();
app.MapVeterinaria();
app.MapAlimentacion();
app.MapPanel();

// Fallback del SPA: devuelve index.html en las rutas de Angular, pero nunca en /api ni /health.
app.MapFallback(async (HttpContext contexto) =>
{
    if (contexto.Request.Path.StartsWithSegments("/api") ||
        contexto.Request.Path.StartsWithSegments("/health"))
    {
        return Results.NotFound();
    }

    var raizWeb = app.Environment.WebRootPath
        ?? Path.Combine(app.Environment.ContentRootPath, "wwwroot");
    var index = Path.Combine(raizWeb, "index.html");

    return File.Exists(index)
        ? Results.File(index, "text/html; charset=utf-8")
        : Results.NotFound();
});

app.Run();

// Render entrega DATABASE_URL como postgresql://usuario:clave@host:puerto/base.
static string ANpgsql(string url)
{
    var uri = new Uri(url);
    var usuarioClave = uri.UserInfo.Split(':', 2);
    if (usuarioClave.Length != 2)
    {
        throw new InvalidOperationException("DATABASE_URL no incluye usuario y contraseña.");
    }

    var puerto = uri.Port > 0 ? uri.Port : 5432;
    var baseDatos = uri.AbsolutePath.TrimStart('/');

    return $"Host={uri.Host};Port={puerto};Database={baseDatos};" +
           $"Username={Uri.UnescapeDataString(usuarioClave[0])};" +
           $"Password={Uri.UnescapeDataString(usuarioClave[1])};SSL Mode=Prefer";
}
