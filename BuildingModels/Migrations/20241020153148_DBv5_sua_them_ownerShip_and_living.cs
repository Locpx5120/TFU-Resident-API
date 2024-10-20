using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class DBv5_sua_them_ownerShip_and_living : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "OwnerShips",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Livings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OwnerShips_UserId",
                table: "OwnerShips",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Livings_UserId",
                table: "Livings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Livings_User_UserId",
                table: "Livings",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerShips_User_UserId",
                table: "OwnerShips",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livings_User_UserId",
                table: "Livings");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnerShips_User_UserId",
                table: "OwnerShips");

            migrationBuilder.DropIndex(
                name: "IX_OwnerShips_UserId",
                table: "OwnerShips");

            migrationBuilder.DropIndex(
                name: "IX_Livings_UserId",
                table: "Livings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OwnerShips");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Livings");
        }
    }
}
