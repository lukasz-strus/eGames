using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Refactor_Games_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Games",
                newName: "Price_Amount");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Games",
                newName: "Price_Currency");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price_Amount",
                table: "Games",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldPrecision: 18,
                oldScale: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price_Amount",
                table: "Games",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Price_Currency",
                table: "Games",
                newName: "Currency");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Games",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }
    }
}
