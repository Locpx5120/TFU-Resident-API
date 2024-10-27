using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class UpdateTableServiceContract2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "ServiceContracts",
                newName: "Quantity");

            migrationBuilder.AddColumn<bool>(
                name: "IsApprove",
                table: "ServiceContracts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApprove",
                table: "ServiceContracts");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "ServiceContracts",
                newName: "Amount");
        }
    }
}
