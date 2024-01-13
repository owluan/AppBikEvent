using Microsoft.EntityFrameworkCore.Migrations;

namespace BikEvent.API.Migrations
{
    public partial class AlteracaoPropriedadesClasseEvent2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobDescription",
                table: "Events",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
               name: "Description",
               table: "Events",
               newName: "JobDescription");
        }
    }
}
