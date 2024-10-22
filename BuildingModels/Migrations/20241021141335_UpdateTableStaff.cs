using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class UpdateTableStaff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Staff",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staff_RoleId",
                table: "Staff",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Roles_RoleId",
                table: "Staff",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Roles_RoleId",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Staff_RoleId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Staff");
        }
    }
}
