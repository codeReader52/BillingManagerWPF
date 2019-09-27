using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BillingManagement.Migrations
{
    public partial class billingtest3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Attachement",
                table: "BillInfos",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAlreadyPaid",
                table: "BillInfos",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachement",
                table: "BillInfos");

            migrationBuilder.DropColumn(
                name: "IsAlreadyPaid",
                table: "BillInfos");
        }
    }
}
