using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiCasino.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cartas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroCarta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cartas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Participantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreParticipante = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participantes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rifas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRifa = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Fecha_Realizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rifas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParticipanteRifaCarta",
                columns: table => new
                {
                    IdParticipante = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdRifa = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdCarta = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParticipanteId = table.Column<int>(type: "int", nullable: true),
                    RifaId = table.Column<int>(type: "int", nullable: true),
                    CartaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipanteRifaCarta", x => new { x.IdParticipante, x.IdRifa, x.IdCarta });
                    table.ForeignKey(
                        name: "FK_ParticipanteRifaCarta_Cartas_CartaId",
                        column: x => x.CartaId,
                        principalTable: "Cartas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParticipanteRifaCarta_Participantes_ParticipanteId",
                        column: x => x.ParticipanteId,
                        principalTable: "Participantes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParticipanteRifaCarta_Rifas_RifaId",
                        column: x => x.RifaId,
                        principalTable: "Rifas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Premios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombrePremio = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NumPremio = table.Column<int>(type: "int", nullable: false),
                    RifaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Premios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Premios_Rifas_RifaId",
                        column: x => x.RifaId,
                        principalTable: "Rifas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParticipanteRifaCarta_CartaId",
                table: "ParticipanteRifaCarta",
                column: "CartaId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipanteRifaCarta_ParticipanteId",
                table: "ParticipanteRifaCarta",
                column: "ParticipanteId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipanteRifaCarta_RifaId",
                table: "ParticipanteRifaCarta",
                column: "RifaId");

            migrationBuilder.CreateIndex(
                name: "IX_Premios_RifaId",
                table: "Premios",
                column: "RifaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParticipanteRifaCarta");

            migrationBuilder.DropTable(
                name: "Premios");

            migrationBuilder.DropTable(
                name: "Cartas");

            migrationBuilder.DropTable(
                name: "Participantes");

            migrationBuilder.DropTable(
                name: "Rifas");
        }
    }
}
