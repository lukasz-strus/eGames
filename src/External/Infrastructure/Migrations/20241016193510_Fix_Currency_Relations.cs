using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Currency_Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Currency_Price_Price_CurrencyId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Currency_Price_Price_CurrencyId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_Price_Price_CurrencyId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_Games_Price_Price_CurrencyId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Price_Price_CurrencyId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Price_Price_CurrencyId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "Price_CurrencyId",
                table: "OrderItems",
                newName: "Price_Currency");

            migrationBuilder.RenameColumn(
                name: "Price_CurrencyId",
                table: "Games",
                newName: "Price_Currency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price_Currency",
                table: "OrderItems",
                newName: "Price_CurrencyId");

            migrationBuilder.RenameColumn(
                name: "Price_Currency",
                table: "Games",
                newName: "Price_CurrencyId");

            migrationBuilder.AddColumn<int>(
                name: "Price_Price_CurrencyId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Price_Price_CurrencyId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_Price_Price_CurrencyId",
                table: "OrderItems",
                column: "Price_Price_CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Price_Price_CurrencyId",
                table: "Games",
                column: "Price_Price_CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Currency_Price_Price_CurrencyId",
                table: "Games",
                column: "Price_Price_CurrencyId",
                principalTable: "Currency",
                principalColumn: "Value",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Currency_Price_Price_CurrencyId",
                table: "OrderItems",
                column: "Price_Price_CurrencyId",
                principalTable: "Currency",
                principalColumn: "Value",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
