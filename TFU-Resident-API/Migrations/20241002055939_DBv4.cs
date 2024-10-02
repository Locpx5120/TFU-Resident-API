using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFU_Resident_API.Migrations
{
    public partial class DBv4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChangePassword",
                schema: "software",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7fe6bd0c-afe5-489d-982d-6f107f1d06fd"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 2, 12, 59, 38, 828, DateTimeKind.Local).AddTicks(3346), new DateTime(2024, 10, 2, 12, 59, 38, 828, DateTimeKind.Local).AddTicks(3346) });

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("98ae41e1-3379-4193-9856-1c9162a8c9c2"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 2, 12, 59, 38, 828, DateTimeKind.Local).AddTicks(3334), new DateTime(2024, 10, 2, 12, 59, 38, 828, DateTimeKind.Local).AddTicks(3343) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChangePassword",
                schema: "software",
                table: "Users");

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7fe6bd0c-afe5-489d-982d-6f107f1d06fd"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 2, 11, 56, 17, 523, DateTimeKind.Local).AddTicks(5994), new DateTime(2024, 10, 2, 11, 56, 17, 523, DateTimeKind.Local).AddTicks(5995) });

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("98ae41e1-3379-4193-9856-1c9162a8c9c2"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 2, 11, 56, 17, 523, DateTimeKind.Local).AddTicks(5976), new DateTime(2024, 10, 2, 11, 56, 17, 523, DateTimeKind.Local).AddTicks(5992) });
        }
    }
}
