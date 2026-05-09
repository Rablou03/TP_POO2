using Microsoft.EntityFrameworkCore.Migrations;

namespace WPFClassificationGrainsDeBles.Migrations
{
    public partial class AjoutAuteur : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuteurNom",
                table: "donnees",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuteurNom",
                table: "donnees");
        }
    }
}
