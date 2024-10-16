using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFU_Resident_API.Migrations
{
    public partial class Dbv6_updateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7fe6bd0c-afe5-489d-982d-6f107f1d06fd"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 16, 5, 47, 43, 0, DateTimeKind.Local).AddTicks(244), new DateTime(2024, 10, 16, 5, 47, 43, 0, DateTimeKind.Local).AddTicks(244) });

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("98ae41e1-3379-4193-9856-1c9162a8c9c2"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 16, 5, 47, 43, 0, DateTimeKind.Local).AddTicks(231), new DateTime(2024, 10, 16, 5, 47, 43, 0, DateTimeKind.Local).AddTicks(241) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7fe6bd0c-afe5-489d-982d-6f107f1d06fd"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 16, 5, 46, 25, 355, DateTimeKind.Local).AddTicks(1765), new DateTime(2024, 10, 16, 5, 46, 25, 355, DateTimeKind.Local).AddTicks(1766) });

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("98ae41e1-3379-4193-9856-1c9162a8c9c2"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 16, 5, 46, 25, 355, DateTimeKind.Local).AddTicks(1753), new DateTime(2024, 10, 16, 5, 46, 25, 355, DateTimeKind.Local).AddTicks(1763) });
        }
    }
}
