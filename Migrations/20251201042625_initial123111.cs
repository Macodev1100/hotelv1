using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotelv1.Migrations
{
    /// <inheritdoc />
    public partial class initial123111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "Habitaciones",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "FotoUrl",
                table: "Habitaciones",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObservacionEstado",
                table: "Habitaciones",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Ocupada",
                table: "Habitaciones",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoUrl",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "ObservacionEstado",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "Ocupada",
                table: "Habitaciones");

            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "Habitaciones",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }
    }
}
