using Microsoft.EntityFrameworkCore;
using ZooLogic.Api.Models;

namespace ZooLogic.Api.Data;

public class ZooDbContext : DbContext
{
    public ZooDbContext(DbContextOptions<ZooDbContext> options)
        : base(options)
    {
    }

    public DbSet<Especie> Especies => Set<Especie>();
    public DbSet<Animal> Animales => Set<Animal>();
    public DbSet<ConsultaVeterinaria> Consultas => Set<ConsultaVeterinaria>();
    public DbSet<Tratamiento> Tratamientos => Set<Tratamiento>();
    public DbSet<AlertaSalud> AlertasSalud => Set<AlertaSalud>();
    public DbSet<Alimento> Alimentos => Set<Alimento>();
    public DbSet<Dieta> Dietas => Set<Dieta>();
    public DbSet<DietaLinea> DietaLineas => Set<DietaLinea>();
    public DbSet<RegistroAlimentacion> RegistrosAlimentacion => Set<RegistroAlimentacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Especie>(e =>
        {
            e.Property(x => x.NombreComun).HasMaxLength(80).IsRequired();
            e.Property(x => x.NombreCientifico).HasMaxLength(120).IsRequired();
            e.Property(x => x.Grupo).HasMaxLength(20).IsRequired();
            e.Property(x => x.TipoDieta).HasMaxLength(20).IsRequired();
            e.Property(x => x.EstadoConservacion).HasMaxLength(40).IsRequired();

            e.HasIndex(x => x.NombreCientifico).IsUnique();
            e.HasIndex(x => x.Grupo);
        });

        modelBuilder.Entity<Animal>(e =>
        {
            e.Property(x => x.Nombre).HasMaxLength(60).IsRequired();
            e.Property(x => x.Sexo).HasMaxLength(10).IsRequired();
            e.Property(x => x.PesoKg).HasPrecision(8, 2);
            e.Property(x => x.Recinto).HasMaxLength(60).IsRequired();
            e.Property(x => x.Estado).HasMaxLength(20).IsRequired().HasDefaultValue("Activo");
            e.Property(x => x.FotoUrl).HasMaxLength(300);

            e.HasOne(x => x.Especie)
                .WithMany(x => x.Animales)
                .HasForeignKey(x => x.EspecieId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.EspecieId);
            e.HasIndex(x => x.Estado);
            e.HasIndex(x => x.Nombre);
        });

        modelBuilder.Entity<ConsultaVeterinaria>(e =>
        {
            e.Property(x => x.Veterinario).HasMaxLength(100).IsRequired();
            e.Property(x => x.Motivo).HasMaxLength(200).IsRequired();
            e.Property(x => x.Diagnostico).HasMaxLength(1000).IsRequired();
            e.Property(x => x.PesoKg).HasPrecision(8, 2);
            e.Property(x => x.Temperatura).HasPrecision(4, 2);

            e.HasOne(x => x.Animal)
                .WithMany(x => x.Consultas)
                .HasForeignKey(x => x.AnimalId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.AnimalId, x.Fecha });
            e.HasIndex(x => x.Fecha);
        });

        modelBuilder.Entity<Tratamiento>(e =>
        {
            e.Property(x => x.Medicamento).HasMaxLength(100).IsRequired();
            e.Property(x => x.Dosis).HasMaxLength(100).IsRequired();
            e.Property(x => x.Notas).HasMaxLength(500);

            e.HasOne(x => x.Consulta)
                .WithMany(x => x.Tratamientos)
                .HasForeignKey(x => x.ConsultaId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.ConsultaId);
            e.HasIndex(x => x.FechaFin);
        });

        modelBuilder.Entity<AlertaSalud>(e =>
        {
            e.Property(x => x.Gravedad).HasMaxLength(20).IsRequired();
            e.Property(x => x.Tipo).HasMaxLength(60).IsRequired();
            e.Property(x => x.Mensaje).HasMaxLength(500).IsRequired();
            e.Property(x => x.Estado).HasMaxLength(20).IsRequired().HasDefaultValue("Abierta");
            e.Property(x => x.Origen).HasMaxLength(40).IsRequired().HasDefaultValue("IA simulada");

            e.HasOne(x => x.Animal)
                .WithMany(x => x.Alertas)
                .HasForeignKey(x => x.AnimalId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.AnimalId, x.Fecha });
            e.HasIndex(x => x.Estado);
            e.HasIndex(x => x.Gravedad);
        });

        modelBuilder.Entity<Alimento>(e =>
        {
            e.Property(x => x.Nombre).HasMaxLength(80).IsRequired();
            e.Property(x => x.Categoria).HasMaxLength(40).IsRequired();
            e.Property(x => x.KcalPorKg).HasPrecision(8, 2);

            e.HasIndex(x => x.Nombre).IsUnique();
            e.HasIndex(x => x.Categoria);
        });

        modelBuilder.Entity<Dieta>(e =>
        {
            e.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
            e.Property(x => x.Estado).HasMaxLength(20).IsRequired().HasDefaultValue("Propuesta");
            e.Property(x => x.ValidadaPor).HasMaxLength(100);
            e.Property(x => x.Origen).HasMaxLength(40).IsRequired().HasDefaultValue("Manual");

            e.HasOne(x => x.Especie)
                .WithMany(x => x.Dietas)
                .HasForeignKey(x => x.EspecieId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.EspecieId, x.Estado });
        });

        modelBuilder.Entity<DietaLinea>(e =>
        {
            e.Property(x => x.CantidadKgDia).HasPrecision(8, 3);
            e.Property(x => x.Frecuencia).HasMaxLength(60).IsRequired();

            e.HasOne(x => x.Dieta)
                .WithMany(x => x.Lineas)
                .HasForeignKey(x => x.DietaId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Alimento)
                .WithMany(x => x.DietaLineas)
                .HasForeignKey(x => x.AlimentoId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.DietaId, x.AlimentoId }).IsUnique();
            e.HasIndex(x => x.AlimentoId);
        });

        modelBuilder.Entity<RegistroAlimentacion>(e =>
        {
            e.Property(x => x.CantidadKg).HasPrecision(8, 3);
            e.Property(x => x.Consumido).HasPrecision(5, 2);

            e.HasOne(x => x.Animal)
                .WithMany(x => x.RegistrosAlimentacion)
                .HasForeignKey(x => x.AnimalId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Alimento)
                .WithMany(x => x.RegistrosAlimentacion)
                .HasForeignKey(x => x.AlimentoId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.AnimalId, x.Fecha });
            e.HasIndex(x => x.AlimentoId);
            e.HasIndex(x => x.Fecha);
        });
    }
}
