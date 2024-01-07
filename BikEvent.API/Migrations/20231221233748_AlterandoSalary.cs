using Microsoft.EntityFrameworkCore.Migrations;

namespace BikEvent.API.Migrations
{
    public partial class AlterandoSalary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Salary",
                table: "Jobs",
                newName: "InitialSalary");

            migrationBuilder.AddColumn<double>(
                name: "FinalSalary",
                table: "Jobs",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalSalary",
                table: "Jobs");

            migrationBuilder.RenameColumn(
               name: "InitialSalary",
               table: "Jobs",
               newName: "Salary");
        }
    }
}
