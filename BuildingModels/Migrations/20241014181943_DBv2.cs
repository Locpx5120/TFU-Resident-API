using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class DBv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThirdPartyContacts_Staff_StaffId",
                table: "ThirdPartyContacts");

            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "ThirdPartyContacts",
                newName: "ThirdPartyId");

            migrationBuilder.RenameIndex(
                name: "IX_ThirdPartyContacts_StaffId",
                table: "ThirdPartyContacts",
                newName: "IX_ThirdPartyContacts_ThirdPartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThirdPartyContacts_ThirdParties_ThirdPartyId",
                table: "ThirdPartyContacts",
                column: "ThirdPartyId",
                principalTable: "ThirdParties",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThirdPartyContacts_ThirdParties_ThirdPartyId",
                table: "ThirdPartyContacts");

            migrationBuilder.RenameColumn(
                name: "ThirdPartyId",
                table: "ThirdPartyContacts",
                newName: "StaffId");

            migrationBuilder.RenameIndex(
                name: "IX_ThirdPartyContacts_ThirdPartyId",
                table: "ThirdPartyContacts",
                newName: "IX_ThirdPartyContacts_StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThirdPartyContacts_Staff_StaffId",
                table: "ThirdPartyContacts",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id");
        }
    }
}
