using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentMS.DAL.Migrations
{
    public partial class UpdatePaymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentAmount",
                table: "Payments",
                type: "decimal(28, 10)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditLimit",
                table: "BalanceProviders",
                type: "decimal(28, 10)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "BalanceClients",
                type: "decimal(28, 10)",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentAmount",
                table: "Payments",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditLimit",
                table: "BalanceProviders",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "BalanceClients",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28, 10)");
        }
    }
}
