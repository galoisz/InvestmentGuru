using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProtfolioPeriod_FinalBudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Prices_StockId",
                table: "Prices");

            migrationBuilder.AddColumn<decimal>(
                name: "FinalBudget",
                table: "ProtfolioPeriods",
                type: "numeric",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prices_StockId",
                table: "Prices",
                column: "StockId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Prices_StockId",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "FinalBudget",
                table: "ProtfolioPeriods");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_StockId",
                table: "Prices",
                column: "StockId");
        }
    }
}
