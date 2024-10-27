using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class UpdateTableInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "Invoices",
                newName: "PaidDate");

            migrationBuilder.AddColumn<bool>(
                name: "PaidStatus",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceContractId",
                table: "Invoices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ServiceContractId",
                table: "Invoices",
                column: "ServiceContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_ServiceContracts_ServiceContractId",
                table: "Invoices",
                column: "ServiceContractId",
                principalTable: "ServiceContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_ServiceContracts_ServiceContractId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_ServiceContractId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "PaidStatus",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ServiceContractId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "PaidDate",
                table: "Invoices",
                newName: "TransactionDate");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Invoices",
                type: "int",
                nullable: true);
        }
    }
}
