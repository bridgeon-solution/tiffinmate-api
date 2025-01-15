using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update_order_subscription_20250115 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "payment_status",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "payment_status",
                table: "order");

            migrationBuilder.AddColumn<int>(
                name: "order_status",
                table: "subscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "fooditem_price",
                table: "orderDetails",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "order_status",
                table: "order",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order_status",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "fooditem_price",
                table: "orderDetails");

            migrationBuilder.DropColumn(
                name: "order_status",
                table: "order");

            migrationBuilder.AddColumn<bool>(
                name: "payment_status",
                table: "subscriptions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "payment_status",
                table: "order",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
