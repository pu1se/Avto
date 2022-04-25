using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentMS.DAL.Migrations
{
    public partial class AddCurrencyExchangeConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyExchangeRates",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "IsBase",
                table: "Currencies");

            migrationBuilder.AlterColumn<string>(
                name: "ToCurrencyCode",
                table: "CurrencyExchangeRates",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "FromCurrencyCode",
                table: "CurrencyExchangeRates",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "ExchangeProvider",
                table: "CurrencyExchangeRates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "CurrencyExchangeRates",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "CurrencyExchangeRates",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql("update CurrencyExchangeRates set Id = newid()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyExchangeRates",
                table: "CurrencyExchangeRates",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CurrencyExchangeConfigs",
                columns: table => new
                {
                    FromCurrencyCode = table.Column<string>(nullable: false),
                    ToCurrencyCode = table.Column<string>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    ExchangeProvider = table.Column<int>(nullable: false),
                    Surcharge = table.Column<decimal>(type: "decimal(26, 10)", nullable: false),
                    CustomRate = table.Column<decimal>(type: "decimal(26, 10)", nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyExchangeConfigs", x => new { x.FromCurrencyCode, x.ToCurrencyCode, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_CurrencyExchangeConfigs_Currencies_FromCurrencyCode",
                        column: x => x.FromCurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyExchangeConfigs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyExchangeConfigs_Currencies_ToCurrencyCode",
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
                name: "IX_CurrencyExchangeRates_ExchangeProvider",
                table: "CurrencyExchangeRates",
                column: "ExchangeProvider");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRates_FromCurrencyCode",
                table: "CurrencyExchangeRates",
                column: "FromCurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRates_OrganizationId",
                table: "CurrencyExchangeRates",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeConfigs_OrganizationId",
                table: "CurrencyExchangeConfigs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeConfigs_ToCurrencyCode",
                table: "CurrencyExchangeConfigs",
                column: "ToCurrencyCode");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyExchangeRates_Organizations_OrganizationId",
                table: "CurrencyExchangeRates",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyExchangeRates_Organizations_OrganizationId",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropTable(
                name: "CurrencyExchangeConfigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyExchangeRates",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyExchangeRates_ExchangeDate",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyExchangeRates_ExchangeProvider",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyExchangeRates_FromCurrencyCode",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyExchangeRates_OrganizationId",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropColumn(
                name: "ExchangeProvider",
                table: "CurrencyExchangeRates");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "CurrencyExchangeRates");

            migrationBuilder.AlterColumn<string>(
                name: "ToCurrencyCode",
                table: "CurrencyExchangeRates",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FromCurrencyCode",
                table: "CurrencyExchangeRates",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Currencies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBase",
                table: "Currencies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyExchangeRates",
                table: "CurrencyExchangeRates",
                columns: new[] { "FromCurrencyCode", "ToCurrencyCode", "ExchangeDate" });
        }
    }
}
