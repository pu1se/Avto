using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentMS.DAL.Migrations
{
    public partial class AddedTableCalculatedCurrencyExchangeRates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalculatedCurrencyExchangeRates",
                columns: table => new
                {
                    FromCurrencyCode = table.Column<string>(nullable: true),
                    ToCurrencyCode = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(26, 10)", nullable: false),
                    ExchangeDate = table.Column<DateTime>(type: "Date", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculatedCurrencyExchangeRates", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_CalculatedCurrencyExchangeRates_Currencies_FromCurrencyCode",
                        column: x => x.FromCurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalculatedCurrencyExchangeRates_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalculatedCurrencyExchangeRates_Currencies_ToCurrencyCode",
                        column: x => x.ToCurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedCurrencyExchangeRates_ExchangeDate",
                table: "CalculatedCurrencyExchangeRates",
                column: "ExchangeDate")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedCurrencyExchangeRates_FromCurrencyCode",
                table: "CalculatedCurrencyExchangeRates",
                column: "FromCurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedCurrencyExchangeRates_OrganizationId",
                table: "CalculatedCurrencyExchangeRates",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedCurrencyExchangeRates_ToCurrencyCode",
                table: "CalculatedCurrencyExchangeRates",
                column: "ToCurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedCurrencyExchangeRates_FromCurrencyCode_ToCurrencyCode_ExchangeDate_OrganizationId",
                table: "CalculatedCurrencyExchangeRates",
                columns: new[] { "FromCurrencyCode", "ToCurrencyCode", "ExchangeDate", "OrganizationId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalculatedCurrencyExchangeRates");
        }
    }
}
