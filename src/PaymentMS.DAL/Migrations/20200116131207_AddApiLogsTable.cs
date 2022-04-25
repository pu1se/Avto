using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentMS.DAL.Migrations
{
    public partial class AddApiLogsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Logs = table.Column<string>(maxLength: 8192, nullable: true),
                    PathToAction = table.Column<string>(maxLength: 1024, nullable: true),
                    HttpMethod = table.Column<string>(maxLength: 32, nullable: true),
                    ResponseCode = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiLogs");
        }
    }
}
