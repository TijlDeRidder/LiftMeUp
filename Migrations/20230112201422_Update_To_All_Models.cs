﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiftMeUp.Migrations
{
    public partial class Update_To_All_Models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Lift_stationId",
                table: "Lift",
                column: "stationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lift_Station_stationId",
                table: "Lift",
                column: "stationId",
                principalTable: "Station",
                principalColumn: "stationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lift_Station_stationId",
                table: "Lift");

            migrationBuilder.DropIndex(
                name: "IX_Lift_stationId",
                table: "Lift");
        }
    }
}
