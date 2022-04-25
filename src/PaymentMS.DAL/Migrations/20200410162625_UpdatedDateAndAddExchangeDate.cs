using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentMS.DAL.Migrations
{
    public partial class UpdatedDateAndAddExchangeDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "PaymentSendingWays",
                newName: "CreatedDateUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "PaymentSendingWays",
                newName: "LastUpdatedDateUtc");


            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Payments",
                newName: "CreatedDateUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Payments",
                newName: "LastUpdatedDateUtc");
            

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "PaymentReceivingWays",
                newName: "CreatedDateUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "PaymentReceivingWays",
                newName: "LastUpdatedDateUtc");

            

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Organizations",
                newName: "CreatedDateUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Organizations",
                newName: "LastUpdatedDateUtc");

            

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Currencies",
                newName: "CreatedDateUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Currencies",
                newName: "LastUpdatedDateUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "BalanceProviders",
                newName: "CreatedDateUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "BalanceProviders",
                newName: "LastUpdatedDateUtc");


            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "BalanceClients",
                newName: "CreatedDateUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "BalanceClients",
                newName: "LastUpdatedDateUtc");

            
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "ApiLogs",
                newName: "CreatedDateUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "ApiLogs",
                newName: "LastUpdatedDateUtc");


            migrationBuilder.AddColumn<DateTime>(
                name: "ExchangeDate",
                table: "CurrencyExchangeRates",
                nullable: false,
                defaultValue: DateTime.MinValue);

            migrationBuilder.Sql(@"delete from CurrencyExchangeRates
                                where ExchangeDate = '0001-01-01 00:00:00.0000000'");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyExchangeRates",
                table: "CurrencyExchangeRates");
            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyExchangeRates",
                table: "CurrencyExchangeRates",
                columns: new[] { "FromCurrencyCode", "ToCurrencyCode", "ExchangeDate" });


            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "CurrencyExchangeRates",
                newName: "CreatedDateUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "CurrencyExchangeRates",
                newName: "LastUpdatedDateUtc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyExchangeRates",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropColumn(
                name: "ExchangeDate",
                table: "CurrencyExchangeRates");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDateUtc",
                table: "PaymentSendingWays",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateUtc",
                table: "PaymentSendingWays",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDateUtc",
                table: "Payments",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateUtc",
                table: "Payments",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDateUtc",
                table: "PaymentReceivingWays",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateUtc",
                table: "PaymentReceivingWays",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDateUtc",
                table: "Organizations",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateUtc",
                table: "Organizations",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDateUtc",
                table: "CurrencyExchangeRates",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateUtc",
                table: "CurrencyExchangeRates",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDateUtc",
                table: "Currencies",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateUtc",
                table: "Currencies",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDateUtc",
                table: "BalanceProviders",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateUtc",
                table: "BalanceProviders",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDateUtc",
                table: "BalanceClients",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateUtc",
                table: "BalanceClients",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDateUtc",
                table: "ApiLogs",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateUtc",
                table: "ApiLogs",
                newName: "CreatedDate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyExchangeRates",
                table: "CurrencyExchangeRates",
                columns: new[] { "FromCurrencyCode", "ToCurrencyCode" });
        }
    }
}
