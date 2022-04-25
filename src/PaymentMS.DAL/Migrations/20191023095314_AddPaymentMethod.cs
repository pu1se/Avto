using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentMS.DAL.Migrations
{
    public partial class AddPaymentMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentProvider",
                table: "PaymentSendingWays");

            migrationBuilder.DropColumn(
                name: "PaymentProvider",
                table: "PaymentReceivingWays");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "PaymentReceivingWays",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"update PaymentReceivingWays set PaymentMethod = 1 where PaymentMethod = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "PaymentReceivingWays");

            migrationBuilder.AddColumn<int>(
                name: "PaymentProvider",
                table: "PaymentSendingWays",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentProvider",
                table: "PaymentReceivingWays",
                nullable: false,
                defaultValue: 0);
        }
    }
}
