using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CharacterHub.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personajes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Videojuego = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Clase = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nivel = table.Column<int>(type: "int", nullable: false),
                    Poder = table.Column<int>(type: "int", nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false),
                    ImagenUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personajes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personajes");
        }
    }
}
