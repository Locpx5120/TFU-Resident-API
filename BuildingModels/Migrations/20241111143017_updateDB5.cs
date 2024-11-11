using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class updateDB5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThirdParties_Apartments_ApartmentId",
                table: "ThirdParties");

            migrationBuilder.DropIndex(
                name: "IX_ThirdParties_ApartmentId",
                table: "ThirdParties");

            migrationBuilder.DropColumn(
                name: "ApartmentId",
                table: "ThirdParties");

            migrationBuilder.AddColumn<Guid>(
                name: "ApartmentId",
                table: "ThirdPartyContacts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ThirdPartyContacts_ApartmentId",
                table: "ThirdPartyContacts",
                column: "ApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThirdPartyContacts_Apartments_ApartmentId",
                table: "ThirdPartyContacts",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThirdPartyContacts_Apartments_ApartmentId",
                table: "ThirdPartyContacts");

            migrationBuilder.DropIndex(
                name: "IX_ThirdPartyContacts_ApartmentId",
                table: "ThirdPartyContacts");

            migrationBuilder.DropColumn(
                name: "ApartmentId",
                table: "ThirdPartyContacts");

            migrationBuilder.AddColumn<Guid>(
                name: "ApartmentId",
                table: "ThirdParties",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ThirdParties_ApartmentId",
                table: "ThirdParties",
                column: "ApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThirdParties_Apartments_ApartmentId",
                table: "ThirdParties",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
