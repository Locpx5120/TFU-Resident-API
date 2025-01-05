using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class updateDB20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "ThirdPartyContacts",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileId",
                table: "ThirdPartyContacts");
        }
    }
}
