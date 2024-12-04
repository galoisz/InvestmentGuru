using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProtfolioPeriodGraph : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProtfolioPeriodGraphs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProtfolioPeriodId = table.Column<Guid>(type: "uuid", nullable: false),
                    Graphdata = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtfolioPeriodGraphs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtfolioPeriodGraphs_ProtfolioPeriods_ProtfolioPeriodId",
                        column: x => x.ProtfolioPeriodId,
                        principalTable: "ProtfolioPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProtfolioPeriodGraphs_ProtfolioPeriodId",
                table: "ProtfolioPeriodGraphs",
                column: "ProtfolioPeriodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProtfolioPeriodGraphs");
        }
    }
}
