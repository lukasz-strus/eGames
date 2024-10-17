using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Money : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.RenameColumn(
                name: "Price_Amount",
                table: "OrderItems",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Price_Currency",
                table: "OrderItems",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "Price_Amount",
                table: "Games",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Price_Currency",
                table: "Games",
                newName: "Currency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "OrderItems",
                newName: "Price_Amount");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "OrderItems",
                newName: "Price_Currency");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Games",
                newName: "Price_Amount");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Games",
                newName: "Price_Currency");

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Value = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Value);
                });

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Value", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "USD", "Dollar" },
                    { 2, "EUR", "Euro" },
                    { 3, "PLN", "Polish zloty" }
                });
        }
    }
}
