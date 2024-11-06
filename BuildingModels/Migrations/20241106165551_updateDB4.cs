using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class updateDB4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Livings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Livings");

            migrationBuilder.AddColumn<Guid>(
                name: "LivingId",
                table: "ServiceContracts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceContracts_LivingId",
                table: "ServiceContracts",
                column: "LivingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceContracts_Livings_LivingId",
                table: "ServiceContracts",
                column: "LivingId",
                principalTable: "Livings",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceContracts_Livings_LivingId",
                table: "ServiceContracts");

            migrationBuilder.DropIndex(
                name: "IX_ServiceContracts_LivingId",
                table: "ServiceContracts");

            migrationBuilder.DropColumn(
                name: "LivingId",
                table: "ServiceContracts");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Livings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Livings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
