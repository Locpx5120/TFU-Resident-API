using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class updateDB9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifies_Assigments_AssigmentId",
                table: "Notifies");

            migrationBuilder.DropIndex(
                name: "IX_Notifies_AssigmentId",
                table: "Notifies");

            migrationBuilder.DropColumn(
                name: "AssigmentId",
                table: "Notifies");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Notifies",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "Notifies",
                newName: "Date");

            migrationBuilder.AddColumn<Guid>(
                name: "BuidingId",
                table: "Notifies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "LongContent",
                table: "Notifies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Notifies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ShortContent",
                table: "Notifies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Notifies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuidingId",
                table: "Notifies");

            migrationBuilder.DropColumn(
                name: "LongContent",
                table: "Notifies");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Notifies");

            migrationBuilder.DropColumn(
                name: "ShortContent",
                table: "Notifies");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notifies");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Notifies",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Notifies",
                newName: "EndTime");

            migrationBuilder.AddColumn<Guid>(
                name: "AssigmentId",
                table: "Notifies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifies_AssigmentId",
                table: "Notifies",
                column: "AssigmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifies_Assigments_AssigmentId",
                table: "Notifies",
                column: "AssigmentId",
                principalTable: "Assigments",
                principalColumn: "Id");
        }
    }
}
