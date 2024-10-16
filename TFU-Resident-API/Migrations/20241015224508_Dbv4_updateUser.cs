using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFU_Resident_API.Migrations
{
    public partial class Dbv4_updateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CCCD",
                schema: "software",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Gender",
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
                values: new object[] { new DateTime(2024, 10, 16, 5, 45, 8, 223, DateTimeKind.Local).AddTicks(1053), new DateTime(2024, 10, 16, 5, 45, 8, 223, DateTimeKind.Local).AddTicks(1053) });

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("98ae41e1-3379-4193-9856-1c9162a8c9c2"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 16, 5, 45, 8, 223, DateTimeKind.Local).AddTicks(1040), new DateTime(2024, 10, 16, 5, 45, 8, 223, DateTimeKind.Local).AddTicks(1050) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CCCD",
                schema: "software",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "software",
                table: "Users");

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7fe6bd0c-afe5-489d-982d-6f107f1d06fd"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 15, 7, 5, 26, 142, DateTimeKind.Local).AddTicks(9424), new DateTime(2024, 10, 15, 7, 5, 26, 142, DateTimeKind.Local).AddTicks(9424) });

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("98ae41e1-3379-4193-9856-1c9162a8c9c2"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 15, 7, 5, 26, 142, DateTimeKind.Local).AddTicks(9412), new DateTime(2024, 10, 15, 7, 5, 26, 142, DateTimeKind.Local).AddTicks(9420) });
        }
    }
}
