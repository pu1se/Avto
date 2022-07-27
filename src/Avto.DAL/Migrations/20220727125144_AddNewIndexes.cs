using Microsoft.EntityFrameworkCore.Migrations;

namespace Avto.DAL.Migrations
{
    public partial class AddNewIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyExchangeRates",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyExchangeRates_ExchangeDate",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Logs",
                table: "_Logs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyExchangeRates",
                table: "CurrencyExchangeRates",
                column: "Id")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Logs",
                table: "_Logs",
                column: "Id")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRates_CreatedDateUtc",
                table: "CurrencyExchangeRates",
                column: "CreatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRates_ExchangeDate",
                table: "CurrencyExchangeRates",
                column: "ExchangeDate")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRates_LastUpdatedDateUtc",
                table: "CurrencyExchangeRates",
                column: "LastUpdatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRates_FromCurrencyCode_ToCurrencyCode_ExchangeDate",
                table: "CurrencyExchangeRates",
                columns: new[] { "FromCurrencyCode", "ToCurrencyCode", "ExchangeDate" },
                unique: true,
                filter: "[FromCurrencyCode] IS NOT NULL AND [ToCurrencyCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX__Logs_CreatedDateUtc",
                table: "_Logs",
                column: "CreatedDateUtc")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX__Logs_ExecutionTimeInMillSec",
                table: "_Logs",
                column: "ExecutionTimeInMillSec");

            migrationBuilder.CreateIndex(
                name: "IX__Logs_HttpMethod",
                table: "_Logs",
                column: "HttpMethod");

            migrationBuilder.CreateIndex(
                name: "IX__Logs_LastUpdatedDateUtc",
                table: "_Logs",
                column: "LastUpdatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX__Logs_PathToAction",
                table: "_Logs",
                column: "PathToAction");

            migrationBuilder.CreateIndex(
                name: "IX__Logs_ResponseCode",
                table: "_Logs",
                column: "ResponseCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyExchangeRates",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyExchangeRates_CreatedDateUtc",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyExchangeRates_ExchangeDate",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyExchangeRates_LastUpdatedDateUtc",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyExchangeRates_FromCurrencyCode_ToCurrencyCode_ExchangeDate",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Logs",
                table: "_Logs");

            migrationBuilder.DropIndex(
                name: "IX__Logs_CreatedDateUtc",
                table: "_Logs");

            migrationBuilder.DropIndex(
                name: "IX__Logs_ExecutionTimeInMillSec",
                table: "_Logs");

            migrationBuilder.DropIndex(
                name: "IX__Logs_HttpMethod",
                table: "_Logs");

            migrationBuilder.DropIndex(
                name: "IX__Logs_LastUpdatedDateUtc",
                table: "_Logs");

            migrationBuilder.DropIndex(
                name: "IX__Logs_PathToAction",
                table: "_Logs");

            migrationBuilder.DropIndex(
                name: "IX__Logs_ResponseCode",
                table: "_Logs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyExchangeRates",
                table: "CurrencyExchangeRates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Logs",
                table: "_Logs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRates_ExchangeDate",
                table: "CurrencyExchangeRates",
                column: "ExchangeDate");
        }
    }
}
