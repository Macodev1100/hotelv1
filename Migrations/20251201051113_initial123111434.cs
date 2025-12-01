using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotelv1.Migrations
{
    /// <inheritdoc />
    public partial class initial123111434 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReservaServicios",
                columns: table => new
                {
                    ReservaServicioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservaId = table.Column<int>(type: "int", nullable: false),
                    ServicioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaServicios", x => x.ReservaServicioId);
                    table.ForeignKey(
                        name: "FK_ReservaServicios_Reservas_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reservas",
                        principalColumn: "ReservaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservaServicios_Servicios_ServicioId",
                        column: x => x.ServicioId,
                        principalTable: "Servicios",
                        principalColumn: "ServicioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservaServicios_ReservaId",
                table: "ReservaServicios",
                column: "ReservaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaServicios_ServicioId",
                table: "ReservaServicios",
                column: "ServicioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservaServicios");
        }
    }
}
