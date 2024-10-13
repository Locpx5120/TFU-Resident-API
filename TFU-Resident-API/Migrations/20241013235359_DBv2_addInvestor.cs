using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFU_Resident_API.Migrations
{
    public partial class DBv2_addInvestor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer",
                schema: "software");

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                schema: "software",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumberCccd",
                schema: "software",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvestorName",
                schema: "software",
                table: "Investors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7fe6bd0c-afe5-489d-982d-6f107f1d06fd"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 14, 6, 53, 59, 531, DateTimeKind.Local).AddTicks(6241), new DateTime(2024, 10, 14, 6, 53, 59, 531, DateTimeKind.Local).AddTicks(6241) });

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("98ae41e1-3379-4193-9856-1c9162a8c9c2"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 14, 6, 53, 59, 531, DateTimeKind.Local).AddTicks(6205), new DateTime(2024, 10, 14, 6, 53, 59, 531, DateTimeKind.Local).AddTicks(6237) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                schema: "software",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NumberCccd",
                schema: "software",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "InvestorName",
                schema: "software",
                table: "Investors");

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "software",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodePostion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsertedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "software",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7fe6bd0c-afe5-489d-982d-6f107f1d06fd"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 14, 0, 47, 19, 868, DateTimeKind.Local).AddTicks(7120), new DateTime(2024, 10, 14, 0, 47, 19, 868, DateTimeKind.Local).AddTicks(7121) });

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("98ae41e1-3379-4193-9856-1c9162a8c9c2"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 14, 0, 47, 19, 868, DateTimeKind.Local).AddTicks(7108), new DateTime(2024, 10, 14, 0, 47, 19, 868, DateTimeKind.Local).AddTicks(7117) });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UserId",
                schema: "software",
                table: "Customer",
                column: "UserId");
        }
    }
}
