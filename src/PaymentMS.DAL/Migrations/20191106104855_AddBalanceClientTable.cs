using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentMS.DAL.Migrations
{
    public partial class AddBalanceClientTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BalanceClientEntityId",
                table: "PaymentSendingWays",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BalanceClients",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Currency = table.Column<string>(maxLength: 8, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    BalanceProviderId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BalanceClients_BalanceProviders_BalanceProviderId",
                        column: x => x.BalanceProviderId,
                        principalTable: "BalanceProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BalanceClients_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSendingWays_BalanceClientEntityId",
                table: "PaymentSendingWays",
                column: "BalanceClientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceClients_BalanceProviderId",
                table: "BalanceClients",
                column: "BalanceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceClients_OrganizationId",
                table: "BalanceClients",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentSendingWays_BalanceClients_BalanceClientEntityId",
                table: "PaymentSendingWays",
                column: "BalanceClientEntityId",
                principalTable: "BalanceClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentSendingWays_BalanceClients_BalanceClientEntityId",
                table: "PaymentSendingWays");

            migrationBuilder.DropTable(
                name: "BalanceClients");

            migrationBuilder.DropIndex(
                name: "IX_PaymentSendingWays_BalanceClientEntityId",
                table: "PaymentSendingWays");

            migrationBuilder.DropColumn(
                name: "BalanceClientEntityId",
                table: "PaymentSendingWays");
        }
    }
}
