using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentMS.DAL.Migrations
{
    public partial class AddFieldToPaymentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Payments",
                maxLength: 2048,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Payments");
        }
    }
}
