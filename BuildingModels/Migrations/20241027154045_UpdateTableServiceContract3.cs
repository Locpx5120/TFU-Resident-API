using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class UpdateTableServiceContract3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PackageServiceId",
                table: "ServiceContracts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceContracts_PackageServiceId",
                table: "ServiceContracts",
                column: "PackageServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceContracts_PackageServices_PackageServiceId",
                table: "ServiceContracts",
                column: "PackageServiceId",
                principalTable: "PackageServices",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceContracts_PackageServices_PackageServiceId",
                table: "ServiceContracts");

            migrationBuilder.DropIndex(
                name: "IX_ServiceContracts_PackageServiceId",
                table: "ServiceContracts");

            migrationBuilder.DropColumn(
                name: "PackageServiceId",
                table: "ServiceContracts");
        }
    }
}
