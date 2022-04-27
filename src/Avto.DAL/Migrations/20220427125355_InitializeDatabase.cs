using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Avto.DAL.Migrations
{
    public partial class InitializeDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Logs = table.Column<string>(maxLength: 8192, nullable: true),
                    PathToAction = table.Column<string>(maxLength: 1024, nullable: true),
                    HttpMethod = table.Column<string>(maxLength: 32, nullable: true),
                    ResponseCode = table.Column<int>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(nullable: false),
                    ExecutionTimeInMillSec = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Code = table.Column<string>(maxLength: 8, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyExchangeRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FromCurrencyCode = table.Column<string>(nullable: true),
                    ToCurrencyCode = table.Column<string>(nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(26, 10)", nullable: false),
                    ExchangeDate = table.Column<DateTime>(type: "Date", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(nullable: false),
                    OpenDayRate = table.Column<decimal>(type: "decimal(26, 10)", nullable: false),
                    MinDayRate = table.Column<decimal>(type: "decimal(26, 10)", nullable: false),
                    MaxDayRate = table.Column<decimal>(type: "decimal(26, 10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyExchangeRates", x => x.Id);
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
                name: "IX_CurrencyExchangeRates_ExchangeDate",
                table: "CurrencyExchangeRates",
                column: "ExchangeDate");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRates_FromCurrencyCode",
                table: "CurrencyExchangeRates",
                column: "FromCurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRates_ToCurrencyCode",
                table: "CurrencyExchangeRates",
                column: "ToCurrencyCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_Logs");

            migrationBuilder.DropTable(
                name: "CurrencyExchangeRates");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
