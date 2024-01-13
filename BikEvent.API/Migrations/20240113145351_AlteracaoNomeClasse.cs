using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BikEvent.API.Migrations
{
    public partial class AlteracaoNomeClasse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Company = table.Column<string>(nullable: false),
                    JobTitle = table.Column<string>(nullable: false),
                    CityState = table.Column<string>(nullable: false),
                    InitialSalary = table.Column<double>(nullable: false),
                    FinalSalary = table.Column<double>(nullable: false),
                    ContractType = table.Column<string>(nullable: false),
                    TecnologyTools = table.Column<string>(nullable: false),
                    CompanyDescription = table.Column<string>(nullable: true),
                    JobDescription = table.Column<string>(nullable: false),
                    Benefits = table.Column<string>(nullable: true),
                    InterestedSendEmailTo = table.Column<string>(nullable: false),
                    PublicationDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Benefits = table.Column<string>(type: "TEXT", nullable: true),
                    CityState = table.Column<string>(type: "TEXT", nullable: false),
                    Company = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyDescription = table.Column<string>(type: "TEXT", nullable: true),
                    ContractType = table.Column<string>(type: "TEXT", nullable: false),
                    FinalSalary = table.Column<double>(type: "REAL", nullable: false),
                    InitialSalary = table.Column<double>(type: "REAL", nullable: false),
                    InterestedSendEmailTo = table.Column<string>(type: "TEXT", nullable: false),
                    JobDescription = table.Column<string>(type: "TEXT", nullable: false),
                    JobTitle = table.Column<string>(type: "TEXT", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TecnologyTools = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_UserId",
                table: "Jobs",
                column: "UserId");
        }
    }
}
