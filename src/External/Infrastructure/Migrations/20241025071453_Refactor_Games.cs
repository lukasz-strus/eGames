using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Refactor_Games : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Games_BaseGameId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "BaseGameId",
                table: "Games",
                newName: "FullGameId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_BaseGameId",
                table: "Games",
                newName: "IX_Games_FullGameId");

            migrationBuilder.AlterColumn<long>(
                name: "PeriodInDays",
                table: "Games",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Games_FullGameId",
                table: "Games",
                column: "FullGameId",
                principalTable: "Games",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Games_FullGameId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "FullGameId",
                table: "Games",
                newName: "BaseGameId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_FullGameId",
                table: "Games",
                newName: "IX_Games_BaseGameId");

            migrationBuilder.AlterColumn<int>(
                name: "PeriodInDays",
                table: "Games",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Games_BaseGameId",
                table: "Games",
                column: "BaseGameId",
                principalTable: "Games",
                principalColumn: "Id");
        }
    }
}
