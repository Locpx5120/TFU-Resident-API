using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFU_Resident_API.Migrations
{
    public partial class Dbv9_updateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buildings",
                schema: "software");

            migrationBuilder.AddColumn<string>(
                name: "ConnectionString",
                schema: "software",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7fe6bd0c-afe5-489d-982d-6f107f1d06fd"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 28, 20, 45, 21, 928, DateTimeKind.Local).AddTicks(9937), new DateTime(2024, 11, 28, 20, 45, 21, 928, DateTimeKind.Local).AddTicks(9938) });

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("98ae41e1-3379-4193-9856-1c9162a8c9c2"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 28, 20, 45, 21, 928, DateTimeKind.Local).AddTicks(9918), new DateTime(2024, 11, 28, 20, 45, 21, 928, DateTimeKind.Local).AddTicks(9934) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionString",
                schema: "software",
                table: "Projects");

            migrationBuilder.CreateTable(
                name: "Buildings",
                schema: "software",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConnectionString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsertedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    MaxNumberApartments = table.Column<double>(type: "float", nullable: false),
                    MaxNumberResidents = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buildings_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "software",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7fe6bd0c-afe5-489d-982d-6f107f1d06fd"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 17, 1, 2, 16, 868, DateTimeKind.Local).AddTicks(8403), new DateTime(2024, 10, 17, 1, 2, 16, 868, DateTimeKind.Local).AddTicks(8404) });

            migrationBuilder.UpdateData(
                schema: "software",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("98ae41e1-3379-4193-9856-1c9162a8c9c2"),
                columns: new[] { "InsertedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 17, 1, 2, 16, 868, DateTimeKind.Local).AddTicks(8391), new DateTime(2024, 10, 17, 1, 2, 16, 868, DateTimeKind.Local).AddTicks(8400) });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_ProjectId",
                schema: "software",
                table: "Buildings",
                column: "ProjectId");
        }
    }
}
