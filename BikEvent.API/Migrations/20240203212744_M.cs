using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BikEvent.API.Migrations
{
    public partial class M : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Company = table.Column<string>(nullable: false),
                    EventTitle = table.Column<string>(nullable: false),
                    CityState = table.Column<string>(nullable: false),
                    EventDate = table.Column<DateTime>(nullable: false),
                    NextEventDate = table.Column<DateTime>(nullable: false),
                    RepeatInterval = table.Column<int>(nullable: false),
                    EventType = table.Column<string>(nullable: false),
                    Tag = table.Column<string>(nullable: false),
                    SocialMedia = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: false),
                    Difficulty = table.Column<string>(nullable: false),
                    Benefits = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: false),
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

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
