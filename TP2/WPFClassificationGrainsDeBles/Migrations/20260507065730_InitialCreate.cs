using Microsoft.EntityFrameworkCore.Migrations;

namespace WPFClassificationGrainsDeBles.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "donnees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    k = table.Column<int>(nullable: false),
                    Distance = table.Column<double>(nullable: false),
                    donnee_Tester = table.Column<string>(nullable: true),
                    precision = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_donnees", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "donnees");
        }
    }
}
