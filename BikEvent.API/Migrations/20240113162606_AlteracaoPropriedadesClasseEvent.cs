using Microsoft.EntityFrameworkCore.Migrations;

namespace BikEvent.API.Migrations
{
    public partial class AlteracaoPropriedadesClasseEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContractType",
                table: "Events",
                newName: "EventType");

            migrationBuilder.RenameColumn(
                name: "JobTitle",
                table: "Events",
                newName: "EventTitle");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                 name: "EventType",
                 table: "Events",
                 newName: "ContractType");

            migrationBuilder.RenameColumn(
                name: "EventTitle",
                table: "Events",
                newName: "JobTitle");
        }
    }
}
