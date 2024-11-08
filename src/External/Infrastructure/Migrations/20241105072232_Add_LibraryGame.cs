using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_LibraryGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderStatuses",
                keyColumn: "Value",
                keyValue: 2);

            migrationBuilder.CreateTable(
                name: "LibraryGames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LicenceKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryGames_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryGames_GameId",
                table: "LibraryGames",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryGames_UserId",
                table: "LibraryGames",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryGames");

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "Value", "Name" },
                values: new object[] { 2, "Canceled" });
        }
    }
}
