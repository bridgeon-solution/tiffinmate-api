using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class add_order_to_payment_history : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_paymentHistory_subscriptions_subscription_id",
                table: "paymentHistory");

            migrationBuilder.RenameColumn(
                name: "subscription_id",
                table: "paymentHistory",
                newName: "order_id");

            migrationBuilder.RenameIndex(
                name: "IX_paymentHistory_subscription_id",
                table: "paymentHistory",
                newName: "IX_paymentHistory_order_id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_paymentHistory_order_order_id",
                table: "paymentHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_paymentHistory_subscriptions_order_id",
                table: "paymentHistory");

            migrationBuilder.RenameColumn(
                name: "order_id",
                table: "paymentHistory",
                newName: "subscription_id");

            migrationBuilder.RenameIndex(
                name: "IX_paymentHistory_order_id",
                table: "paymentHistory",
                newName: "IX_paymentHistory_subscription_id");

            migrationBuilder.AddForeignKey(
                name: "FK_paymentHistory_subscriptions_subscription_id",
                table: "paymentHistory",
                column: "subscription_id",
                principalTable: "subscriptions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
