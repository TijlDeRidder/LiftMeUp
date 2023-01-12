using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiftMeUp.Migrations
{
    public partial class usertest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lift",
                columns: table => new
                {
                    liftId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    stationId = table.Column<int>(type: "int", nullable: false),
                    isWorking = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lift", x => x.liftId);
                });

            migrationBuilder.CreateTable(
                name: "Melding",
                columns: table => new
                {
                    MeldingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    liftId = table.Column<int>(type: "int", nullable: false),
                    stationId = table.Column<int>(type: "int", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    uitleg = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Melding", x => x.MeldingId);
                });

            migrationBuilder.CreateTable(
                name: "Station",
                columns: table => new
                {
                    stationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isAccesible = table.Column<bool>(type: "bit", nullable: false),
                    hasElevator = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Station", x => x.stationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lift");

            migrationBuilder.DropTable(
                name: "Melding");

            migrationBuilder.DropTable(
                name: "Station");
        }
    }
}
