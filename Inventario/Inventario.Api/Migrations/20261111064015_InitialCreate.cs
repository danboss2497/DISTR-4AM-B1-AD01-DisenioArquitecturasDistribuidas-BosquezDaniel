using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bodegas",
                columns: table => new
                {
                    IdBodega = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bodegas", x => x.IdBodega);
                });

            migrationBuilder.CreateTable(
                name: "Existencias",
                columns: table => new
                {
                    IdExistencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    IdBodega = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Existencias", x => x.IdExistencia);
                    table.ForeignKey(
                        name: "FK_Existencias_Bodegas_IdBodega",
                        column: x => x.IdBodega,
                        principalTable: "Bodegas",
                        principalColumn: "IdBodega",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovimientosInventario",
                columns: table => new
                {
                    IdMovimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    IdBodega = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosInventario", x => x.IdMovimiento);
                    table.ForeignKey(
                        name: "FK_MovimientosInventario_Bodegas_IdBodega",
                        column: x => x.IdBodega,
                        principalTable: "Bodegas",
                        principalColumn: "IdBodega",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReservasStock",
                columns: table => new
                {
                    IdReserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    IdBodega = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservasStock", x => x.IdReserva);
                    table.ForeignKey(
                        name: "FK_ReservasStock_Bodegas_IdBodega",
                        column: x => x.IdBodega,
                        principalTable: "Bodegas",
                        principalColumn: "IdBodega",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Existencias_IdBodega",
                table: "Existencias",
                column: "IdBodega");

            migrationBuilder.CreateIndex(
                name: "IX_Existencias_IdProducto_IdBodega",
                table: "Existencias",
                columns: new[] { "IdProducto", "IdBodega" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosInventario_IdBodega",
                table: "MovimientosInventario",
                column: "IdBodega");

            migrationBuilder.CreateIndex(
                name: "IX_ReservasStock_IdBodega",
                table: "ReservasStock",
                column: "IdBodega");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Existencias");

            migrationBuilder.DropTable(
                name: "MovimientosInventario");

            migrationBuilder.DropTable(
                name: "ReservasStock");

            migrationBuilder.DropTable(
                name: "Bodegas");
        }
    }
}
