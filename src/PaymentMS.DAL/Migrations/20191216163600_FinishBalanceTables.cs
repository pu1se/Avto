using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentMS.DAL.Migrations
{
    public partial class FinishBalanceTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentReceivingWays_ReceivingWayId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentSendingWays_BalanceClients_BalanceClientEntityId",
                table: "PaymentSendingWays");

            migrationBuilder.DropIndex(
                name: "IX_PaymentSendingWays_BalanceClientEntityId",
                table: "PaymentSendingWays");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ReceivingWayId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "BalanceClientEntityId",
                table: "PaymentSendingWays");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "PaymentSendingWays");

            migrationBuilder.DropColumn(
                name: "ReceivingWayId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "BalanceClients");

            migrationBuilder.AlterColumn<Guid>(
                name: "SendingWayId",
                table: "Payments",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "BalanceClientId",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "Payments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BalanceClientId",
                table: "Payments",
                column: "BalanceClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_BalanceClients_BalanceClientId",
                table: "Payments",
                column: "BalanceClientId",
                principalTable: "BalanceClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_BalanceClients_BalanceClientId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_BalanceClientId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "BalanceClientId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Payments");

            migrationBuilder.AddColumn<Guid>(
                name: "BalanceClientEntityId",
                table: "PaymentSendingWays",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "PaymentSendingWays",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "SendingWayId",
                table: "Payments",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReceivingWayId",
                table: "Payments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "BalanceClients",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSendingWays_BalanceClientEntityId",
                table: "PaymentSendingWays",
                column: "BalanceClientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ReceivingWayId",
                table: "Payments",
                column: "ReceivingWayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentReceivingWays_ReceivingWayId",
                table: "Payments",
                column: "ReceivingWayId",
                principalTable: "PaymentReceivingWays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentSendingWays_BalanceClients_BalanceClientEntityId",
                table: "PaymentSendingWays",
                column: "BalanceClientEntityId",
                principalTable: "BalanceClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
