using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFU_Resident_API.Migrations
{
    public partial class Dbv3_suProjeect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Permalink",
                schema: "software",
                table: "Projects",
                newName: "Position");

            migrationBuilder.AddColumn<double>(
                name: "MaxNumberApartments",
                schema: "software",
                table: "Buildings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxNumberResidents",
                schema: "software",
                table: "Buildings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxNumberApartments",
                schema: "software",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "MaxNumberResidents",
                schema: "software",
                table: "Buildings");

            migrationBuilder.RenameColumn(
                name: "Position",
                schema: "software",
                table: "Projects",
                newName: "Permalink");

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
    }
}
