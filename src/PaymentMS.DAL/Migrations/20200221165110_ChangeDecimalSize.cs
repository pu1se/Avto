using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentMS.DAL.Migrations
{
    public partial class ChangeDecimalSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentAmount",
                table: "Payments",
                type: "decimal(26, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20, 6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rate",
                table: "CurrencyExchangeRates",
                type: "decimal(26, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20, 6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditLimit",
                table: "BalanceProviders",
                type: "decimal(26, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20, 6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "BalanceClients",
                type: "decimal(26, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20, 6)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentAmount",
                table: "Payments",
                type: "decimal(20, 6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(26, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rate",
                table: "CurrencyExchangeRates",
                type: "decimal(20, 6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(26, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditLimit",
                table: "BalanceProviders",
                type: "decimal(20, 6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(26, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "BalanceClients",
                type: "decimal(20, 6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(26, 10)");
        }
    }
}
