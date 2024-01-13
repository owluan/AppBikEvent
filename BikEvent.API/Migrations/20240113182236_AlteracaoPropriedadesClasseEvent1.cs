using Microsoft.EntityFrameworkCore.Migrations;

namespace BikEvent.API.Migrations
{
    public partial class AlteracaoPropriedadesClasseEvent1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TecnologyTools",
                table: "Events",
                newName: "Tag");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                 name: "Tag",
                 table: "Events",
                 newName: "TecnologyTools");
        }
    }
}
