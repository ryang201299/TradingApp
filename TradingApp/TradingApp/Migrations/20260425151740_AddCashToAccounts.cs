using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCashToAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Cash",
                table: "Accounts",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cash",
                table: "Accounts");
        }
    }
}
