using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HistoricalRatesDal.Migrations
{
    public partial class LocationForTradingDay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "TradingDays",
                type: "nvarchar(40)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "TradingDays");
        }
    }
}
