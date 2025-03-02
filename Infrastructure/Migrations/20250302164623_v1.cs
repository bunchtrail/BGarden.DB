using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LatinName",
                table: "Specimens",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RussianName",
                table: "Specimens",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatinName",
                table: "Specimens");

            migrationBuilder.DropColumn(
                name: "RussianName",
                table: "Specimens");
        }
    }
}
