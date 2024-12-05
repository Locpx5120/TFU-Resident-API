using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class updateDB14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ServiceContractId",
                table: "Assigments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assigments_ServiceContractId",
                table: "Assigments",
                column: "ServiceContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assigments_ServiceContracts_ServiceContractId",
                table: "Assigments",
                column: "ServiceContractId",
                principalTable: "ServiceContracts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assigments_ServiceContracts_ServiceContractId",
                table: "Assigments");

            migrationBuilder.DropIndex(
                name: "IX_Assigments_ServiceContractId",
                table: "Assigments");

            migrationBuilder.DropColumn(
                name: "ServiceContractId",
                table: "Assigments");
        }
    }
}
