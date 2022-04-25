using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentMS.DAL.Migrations
{
    public partial class AddFieldToReceivingWaysTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Configuration",
                table: "PaymentReceivingWays",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Configuration",
                table: "PaymentReceivingWays");
        }
    }
}
