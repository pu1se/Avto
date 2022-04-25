using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentMS.DAL.Migrations
{
    public partial class AddFieldsToExchangeRatesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OpenDayRate",
                table: "CurrencyExchangeRates",
                type: "decimal(26, 10)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpenDayRate",
                table: "CurrencyExchangeRates");
        }
    }
}
