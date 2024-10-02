using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFU_Resident_API.Migrations
{
    public partial class DBv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "software",
                table: "Roles",
                columns: new[] { "Id", "InsertedAt", "InsertedById", "IsActive", "IsDeleted", "Name", "UpdatedAt", "UpdatedById" },
                values: new object[] { new Guid("7fe6bd0c-afe5-489d-982d-6f107f1d06fd"), new DateTime(2024, 10, 2, 11, 56, 17, 523, DateTimeKind.Local).AddTicks(5994), null, true, false, "Admin", new DateTime(2024, 10, 2, 11, 56, 17, 523, DateTimeKind.Local).AddTicks(5995), null });

            migrationBuilder.InsertData(
                schema: "software",
                table: "Roles",
                columns: new[] { "Id", "InsertedAt", "InsertedById", "IsActive", "IsDeleted", "Name", "UpdatedAt", "UpdatedById" },
                values: new object[] { new Guid("98ae41e1-3379-4193-9856-1c9162a8c9c2"), new DateTime(2024, 10, 2, 11, 56, 17, 523, DateTimeKind.Local).AddTicks(5976), null, true, false, "User", new DateTime(2024, 10, 2, 11, 56, 17, 523, DateTimeKind.Local).AddTicks(5992), null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7fe6bd0c-afe5-489d-982d-6f107f1d06fd"));

            migrationBuilder.DeleteData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("98ae41e1-3379-4193-9856-1c9162a8c9c2"));
        }
    }
}
