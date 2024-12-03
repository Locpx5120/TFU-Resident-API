using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class updateDB11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BuidingId",
                table: "Notifies",
                newName: "BuildingId");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Time",
                table: "Notifies",
                type: "time",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BuildingId",
                table: "Notifies",
                newName: "BuidingId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Notifies",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);
        }
    }
}
