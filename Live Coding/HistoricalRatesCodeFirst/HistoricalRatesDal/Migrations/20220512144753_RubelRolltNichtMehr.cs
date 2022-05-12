using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HistoricalRatesDal.Migrations
{
    public partial class RubelRolltNichtMehr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TradingInterupted",
                table: "ExchangeRates",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TradingInterupted",
                table: "ExchangeRates");
        }
    }
}
