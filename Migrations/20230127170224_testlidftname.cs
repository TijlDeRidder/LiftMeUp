using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiftMeUp.Migrations
{
    public partial class testlidftname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "liftName",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "liftName",
                table: "Notification");
        }
    }
}
