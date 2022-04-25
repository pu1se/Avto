using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentMS.DAL.Migrations
{
    public partial class AddCurrencyTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentAmount",
                table: "Payments",
                type: "decimal(20, 6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditLimit",
                table: "BalanceProviders",
                type: "decimal(20, 6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "BalanceClients",
                type: "decimal(20, 6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28, 10)");

            migrationBuilder.AddColumn<long>(
                name: "ExecutionTimeInMilliSec",
                table: "ApiLogs",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Code = table.Column<string>(maxLength: 8, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    IsBase = table.Column<bool>(nullable: false),
                    IsAvailable = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyExchangeRates",
                columns: table => new
                {
                    FromCurrencyCode = table.Column<string>(nullable: false),
                    ToCurrencyCode = table.Column<string>(nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(20, 6)", nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyExchangeRates", x => new { x.FromCurrencyCode, x.ToCurrencyCode });
                    table.ForeignKey(
                        name: "FK_CurrencyExchangeRates_Currencies_FromCurrencyCode",
                        column: x => x.FromCurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyExchangeRates_Currencies_ToCurrencyCode",
                        column: x => x.ToCurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRates_ToCurrencyCode",
                table: "CurrencyExchangeRates",
                column: "ToCurrencyCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyExchangeRates");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropColumn(
                name: "ExecutionTimeInMilliSec",
                table: "ApiLogs");

            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentAmount",
                table: "Payments",
                type: "decimal(28, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20, 6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditLimit",
                table: "BalanceProviders",
                type: "decimal(28, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20, 6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "BalanceClients",
                type: "decimal(28, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20, 6)");
        }
    }
}
