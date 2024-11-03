using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    public partial class DB1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Postions_PostionId",
                table: "Buildings");

            migrationBuilder.DropTable(
                name: "Postions");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_PostionId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "PostionId",
                table: "Buildings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PostionId",
                table: "Buildings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Postions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodePosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsertedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsertedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_PostionId",
                table: "Buildings",
                column: "PostionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Postions_PostionId",
                table: "Buildings",
                column: "PostionId",
                principalTable: "Postions",
                principalColumn: "Id");
        }
    }
}
