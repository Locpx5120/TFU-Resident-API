using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class updateDB13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "ServiceContracts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "NoteDetail",
                table: "ServiceContracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoteFeedbackCuDan",
                table: "ServiceContracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoteFeedbackHanhChinh",
                table: "ServiceContracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoteKyThuat",
                table: "ServiceContracts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoteDetail",
                table: "ServiceContracts");

            migrationBuilder.DropColumn(
                name: "NoteFeedbackCuDan",
                table: "ServiceContracts");

            migrationBuilder.DropColumn(
                name: "NoteFeedbackHanhChinh",
                table: "ServiceContracts");

            migrationBuilder.DropColumn(
                name: "NoteKyThuat",
                table: "ServiceContracts");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "ServiceContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
