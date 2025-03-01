using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class add_subscription_to_payment_history : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_paymentHistory_order_order_id",
                table: "paymentHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_paymentHistory_subscriptions_order_id",
                table: "paymentHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "order_id",
                table: "paymentHistory",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "subscription_id",
                table: "paymentHistory",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_paymentHistory_subscription_id",
                table: "paymentHistory",
                column: "subscription_id");

            migrationBuilder.AddForeignKey(
                name: "FK_paymentHistory_order_order_id",
                table: "paymentHistory",
                column: "order_id",
                principalTable: "order",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_paymentHistory_subscriptions_subscription_id",
                table: "paymentHistory",
                column: "subscription_id",
                principalTable: "subscriptions",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_paymentHistory_order_order_id",
                table: "paymentHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_paymentHistory_subscriptions_subscription_id",
                table: "paymentHistory");

            migrationBuilder.DropIndex(
                name: "IX_paymentHistory_subscription_id",
                table: "paymentHistory");

            migrationBuilder.DropColumn(
                name: "subscription_id",
                table: "paymentHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "order_id",
                table: "paymentHistory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_paymentHistory_order_order_id",
                table: "paymentHistory",
                column: "order_id",
                principalTable: "order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_paymentHistory_subscriptions_order_id",
                table: "paymentHistory",
                column: "order_id",
                principalTable: "subscriptions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
