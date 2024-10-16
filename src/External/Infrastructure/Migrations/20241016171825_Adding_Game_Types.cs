using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Adding_Game_Types : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BaseGameId",
                table: "Games",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Games",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DownloadLink",
                table: "Games",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "FileSize",
                table: "Games",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PeriodInDays",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_BaseGameId",
                table: "Games",
                column: "BaseGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Games_BaseGameId",
                table: "Games",
                column: "BaseGameId",
                principalTable: "Games",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Games_BaseGameId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_BaseGameId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "BaseGameId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "DownloadLink",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PeriodInDays",
                table: "Games");
        }
    }
}
