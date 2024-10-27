using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class AddTableDepartmentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropColumn(
                name: "DepartmentType",
                table: "Apartments");

            migrationBuilder.RenameColumn(
                name: "Floor",
                table: "Apartments",
                newName: "FloorNumber");

            migrationBuilder.AddColumn<Guid>(
                name: "ApartmentTypeId",
                table: "Apartments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ApartmentTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LandArea = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    InsertedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InsertedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_ApartmentTypeId",
                table: "Apartments",
                column: "ApartmentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartments_ApartmentTypes_ApartmentTypeId",
                table: "Apartments",
                column: "ApartmentTypeId",
                principalTable: "ApartmentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartments_ApartmentTypes_ApartmentTypeId",
                table: "Apartments");

            migrationBuilder.DropTable(
                name: "ApartmentTypes");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_ApartmentTypeId",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "ApartmentTypeId",
                table: "Apartments");

            migrationBuilder.RenameColumn(
                name: "FloorNumber",
                table: "Apartments",
                newName: "Floor");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentType",
                table: "Apartments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsertedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsertedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });
        }
    }
}
