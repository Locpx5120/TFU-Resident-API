using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class UdateTableServiceContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Canceled",
                table: "ServiceContracts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastRenewalDate",
                table: "ServiceContracts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RenewStatus",
                table: "ServiceContracts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Canceled",
                table: "ServiceContracts");

            migrationBuilder.DropColumn(
                name: "LastRenewalDate",
                table: "ServiceContracts");

            migrationBuilder.DropColumn(
                name: "RenewStatus",
                table: "ServiceContracts");
        }
    }
}
