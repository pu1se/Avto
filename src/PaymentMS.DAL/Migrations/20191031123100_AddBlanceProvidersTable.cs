using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentMS.DAL.Migrations
{
    public partial class AddBlanceProvidersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BalanceProviderId",
                table: "PaymentReceivingWays",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BalanceProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreditLimit = table.Column<decimal>(nullable: false),
                    Currency = table.Column<string>(maxLength: 8, nullable: false),
                    IsWireTransferIncomeEnabled = table.Column<bool>(nullable: false),
                    IsStripeCardIncomeEnabled = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BalanceProviders_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReceivingWays_BalanceProviderId",
                table: "PaymentReceivingWays",
                column: "BalanceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceProviders_OrganizationId",
                table: "BalanceProviders",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentReceivingWays_BalanceProviders_BalanceProviderId",
                table: "PaymentReceivingWays",
                column: "BalanceProviderId",
                principalTable: "BalanceProviders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentReceivingWays_BalanceProviders_BalanceProviderId",
                table: "PaymentReceivingWays");

            migrationBuilder.DropTable(
                name: "BalanceProviders");

            migrationBuilder.DropIndex(
                name: "IX_PaymentReceivingWays_BalanceProviderId",
                table: "PaymentReceivingWays");

            migrationBuilder.DropColumn(
                name: "BalanceProviderId",
                table: "PaymentReceivingWays");
        }
    }
}
