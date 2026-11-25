using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ZooLogic.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// En Render el puerto llega por la variable PORT.
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

// La cadena de conexión de desarrollo llega por ConnectionStrings__Default.
var connectionString = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Falta la cadena de conexión de PostgreSQL. Define la variable de entorno ConnectionStrings__Default.");
}

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<ZooDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // La API arranca aunque la base de datos no esté disponible: /health responderá 503.
    using var scope = app.Services.CreateScope();
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

app.UseExceptionHandler();
app.UseStatusCodePages();

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

app.Run();
