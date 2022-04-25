using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentMS.DAL.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 512, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentReceivingWays",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PaymentProvider = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentReceivingWays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentReceivingWays_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentSendingWays",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PaymentProvider = table.Column<int>(nullable: false),
                    PaymentMethod = table.Column<int>(nullable: false),
                    Configuration = table.Column<string>(maxLength: 4096, nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    ReceivingWayId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentSendingWays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentSendingWays_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentSendingWays_PaymentReceivingWays_ReceivingWayId",
                        column: x => x.ReceivingWayId,
                        principalTable: "PaymentReceivingWays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExternalId = table.Column<Guid>(nullable: false),
                    TransactionLog = table.Column<string>(maxLength: 8192, nullable: true),
                    PaymentAmount = table.Column<decimal>(nullable: false),
                    PaymentCurrency = table.Column<string>(maxLength: 8, nullable: true),
                    ExternalMetadata = table.Column<string>(maxLength: 2048, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    SendingWayId = table.Column<Guid>(nullable: false),
                    ReceivingWayId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentReceivingWays_ReceivingWayId",
                        column: x => x.ReceivingWayId,
                        principalTable: "PaymentReceivingWays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentSendingWays_SendingWayId",
                        column: x => x.SendingWayId,
                        principalTable: "PaymentSendingWays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReceivingWays_OrganizationId",
                table: "PaymentReceivingWays",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ReceivingWayId",
                table: "Payments",
                column: "ReceivingWayId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SendingWayId",
                table: "Payments",
                column: "SendingWayId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSendingWays_OrganizationId",
                table: "PaymentSendingWays",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSendingWays_ReceivingWayId",
                table: "PaymentSendingWays",
                column: "ReceivingWayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PaymentSendingWays");

            migrationBuilder.DropTable(
                name: "PaymentReceivingWays");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
