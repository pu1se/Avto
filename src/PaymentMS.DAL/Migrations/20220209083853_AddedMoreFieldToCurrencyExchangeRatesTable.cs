using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentMS.DAL.Migrations
{
    public partial class AddedMoreFieldToCurrencyExchangeRatesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MinDayRate",
                table: "CurrencyExchangeRates",
                type: "decimal(26, 10)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxDayRate",
                table: "CurrencyExchangeRates",
                type: "decimal(26, 10)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "HasExtraInfoForRate",
                table: "CurrencyExchangeRates",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasExtraInfoForRate",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropColumn(
                name: "MaxDayRate",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropColumn(
                name: "MinDayRate",
                table: "CurrencyExchangeRates");

            migrationBuilder.AddColumn<decimal>(
                name: "CloseDayRate",
                table: "CurrencyExchangeRates",
                type: "decimal(26, 10)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
