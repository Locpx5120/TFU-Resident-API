using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class AddTablePackageService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "Services");

            migrationBuilder.AddColumn<bool>(
                name: "IsPackageAllowed",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "Services",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "PackageServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DurationInMonth = table.Column<int>(type: "int", nullable: false),
                    AutoRenew = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    InsertedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InsertedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageServices", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageServices");

            migrationBuilder.DropColumn(
                name: "IsPackageAllowed",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "Services");

            migrationBuilder.AddColumn<string>(
                name: "ServiceType",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
