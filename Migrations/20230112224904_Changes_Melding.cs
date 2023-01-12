using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiftMeUp.Migrations
{
    public partial class Changes_Melding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Melding_AspNetUsers_UserId",
                table: "Melding");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Melding",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Melding_AspNetUsers_UserId",
                table: "Melding",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Melding_AspNetUsers_UserId",
                table: "Melding");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Melding",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Melding_AspNetUsers_UserId",
                table: "Melding",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
