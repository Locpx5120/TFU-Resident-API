using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class UpdateTableResident2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Residents_User_UserId",
                table: "Residents");

            migrationBuilder.DropIndex(
                name: "IX_Residents_UserId",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Residents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Residents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Residents_UserId",
                table: "Residents",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Residents_User_UserId",
                table: "Residents",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
