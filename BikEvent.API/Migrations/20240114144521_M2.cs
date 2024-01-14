using Microsoft.EntityFrameworkCore.Migrations;

namespace BikEvent.API.Migrations
{
    public partial class M2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                 name: "InterestedSendEmailTo",
                 table: "Events",
                 newName: "PhoneNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                  name: "PhoneNumber",
                  table: "Events",
                  newName: "InterestedSendEmailTo");
        }
    }
}
