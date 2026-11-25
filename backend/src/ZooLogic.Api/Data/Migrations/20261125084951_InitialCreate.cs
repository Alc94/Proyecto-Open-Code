using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ZooLogic.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alimentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Categoria = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    KcalPorKg = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alimentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Especies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreComun = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    NombreCientifico = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Grupo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TipoDieta = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EstadoConservacion = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Animales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    EspecieId = table.Column<int>(type: "integer", nullable: false),
                    Sexo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    FechaNacimiento = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PesoKg = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    Recinto = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Activo"),
                    FotoUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animales_Especies_EspecieId",
                        column: x => x.EspecieId,
                        principalTable: "Especies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dietas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EspecieId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Propuesta"),
                    ValidadaPor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Origen = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false, defaultValue: "Manual")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dietas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dietas_Especies_EspecieId",
                        column: x => x.EspecieId,
                        principalTable: "Especies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlertasSalud",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimalId = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Gravedad = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Mensaje = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Abierta"),
                    Origen = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false, defaultValue: "IA simulada")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertasSalud", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertasSalud_Animales_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Consultas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimalId = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Veterinario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Motivo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Diagnostico = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    PesoKg = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: true),
                    Temperatura = table.Column<decimal>(type: "numeric(4,2)", precision: 4, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consultas_Animales_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosAlimentacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimalId = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AlimentoId = table.Column<int>(type: "integer", nullable: false),
                    CantidadKg = table.Column<decimal>(type: "numeric(8,3)", precision: 8, scale: 3, nullable: false),
                    Consumido = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosAlimentacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosAlimentacion_Alimentos_AlimentoId",
                        column: x => x.AlimentoId,
                        principalTable: "Alimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrosAlimentacion_Animales_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DietaLineas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DietaId = table.Column<int>(type: "integer", nullable: false),
                    AlimentoId = table.Column<int>(type: "integer", nullable: false),
                    CantidadKgDia = table.Column<decimal>(type: "numeric(8,3)", precision: 8, scale: 3, nullable: false),
                    Frecuencia = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietaLineas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DietaLineas_Alimentos_AlimentoId",
                        column: x => x.AlimentoId,
                        principalTable: "Alimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DietaLineas_Dietas_DietaId",
                        column: x => x.DietaId,
                        principalTable: "Dietas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tratamientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConsultaId = table.Column<int>(type: "integer", nullable: false),
                    Medicamento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Dosis = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FechaInicio = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Notas = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tratamientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tratamientos_Consultas_ConsultaId",
                        column: x => x.ConsultaId,
                        principalTable: "Consultas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertasSalud_AnimalId_Fecha",
                table: "AlertasSalud",
                columns: new[] { "AnimalId", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_AlertasSalud_Estado",
                table: "AlertasSalud",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_AlertasSalud_Gravedad",
                table: "AlertasSalud",
                column: "Gravedad");

            migrationBuilder.CreateIndex(
                name: "IX_Alimentos_Categoria",
                table: "Alimentos",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Alimentos_Nombre",
                table: "Alimentos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animales_EspecieId",
                table: "Animales",
                column: "EspecieId");

            migrationBuilder.CreateIndex(
                name: "IX_Animales_Estado",
                table: "Animales",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Animales_Nombre",
                table: "Animales",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_AnimalId_Fecha",
                table: "Consultas",
                columns: new[] { "AnimalId", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_Fecha",
                table: "Consultas",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_DietaLineas_AlimentoId",
                table: "DietaLineas",
                column: "AlimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_DietaLineas_DietaId_AlimentoId",
                table: "DietaLineas",
                columns: new[] { "DietaId", "AlimentoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dietas_EspecieId_Estado",
                table: "Dietas",
                columns: new[] { "EspecieId", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_Especies_Grupo",
                table: "Especies",
                column: "Grupo");

            migrationBuilder.CreateIndex(
                name: "IX_Especies_NombreCientifico",
                table: "Especies",
                column: "NombreCientifico",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosAlimentacion_AlimentoId",
                table: "RegistrosAlimentacion",
                column: "AlimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosAlimentacion_AnimalId_Fecha",
                table: "RegistrosAlimentacion",
                columns: new[] { "AnimalId", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosAlimentacion_Fecha",
                table: "RegistrosAlimentacion",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_Tratamientos_ConsultaId",
                table: "Tratamientos",
                column: "ConsultaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tratamientos_FechaFin",
                table: "Tratamientos",
                column: "FechaFin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertasSalud");

            migrationBuilder.DropTable(
                name: "DietaLineas");

            migrationBuilder.DropTable(
                name: "RegistrosAlimentacion");

            migrationBuilder.DropTable(
                name: "Tratamientos");

            migrationBuilder.DropTable(
                name: "Dietas");

            migrationBuilder.DropTable(
                name: "Alimentos");

            migrationBuilder.DropTable(
                name: "Consultas");

            migrationBuilder.DropTable(
                name: "Animales");

            migrationBuilder.DropTable(
                name: "Especies");
        }
    }
}
