using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class UpdateTableServiceContract4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApprove",
                table: "ServiceContracts");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "ServiceContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "ServiceContracts");

            migrationBuilder.AddColumn<bool>(
                name: "IsApprove",
                table: "ServiceContracts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
