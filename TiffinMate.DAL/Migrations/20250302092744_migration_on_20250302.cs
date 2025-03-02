using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class migration_on_20250302 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_menus_Menuid",
                table: "order");

            migrationBuilder.DropForeignKey(
                name: "FK_paymentHistory_subscriptions_subscription_id",
                table: "paymentHistory");

            migrationBuilder.DropIndex(
                name: "IX_order_Menuid",
                table: "order");

            migrationBuilder.DropColumn(
                name: "Menuid",
                table: "order");

            migrationBuilder.AlterColumn<string>(
                name: "phone_no",
                table: "ProvidersDetails",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "subscription_id",
                table: "paymentHistory",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "order_id",
                table: "paymentHistory",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "order_type",
                table: "paymentHistory",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_paymentHistory_order_id",
                table: "paymentHistory",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_menu_id",
                table: "order",
                column: "menu_id");

            migrationBuilder.AddForeignKey(
                name: "FK_order_menus_menu_id",
                table: "order",
                column: "menu_id",
                principalTable: "menus",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_order_menus_menu_id",
                table: "order");

            migrationBuilder.DropForeignKey(
                name: "FK_paymentHistory_order_order_id",
                table: "paymentHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_paymentHistory_subscriptions_subscription_id",
                table: "paymentHistory");

            migrationBuilder.DropIndex(
                name: "IX_paymentHistory_order_id",
                table: "paymentHistory");

            migrationBuilder.DropIndex(
                name: "IX_order_menu_id",
                table: "order");

            migrationBuilder.DropColumn(
                name: "order_id",
                table: "paymentHistory");

            migrationBuilder.DropColumn(
                name: "order_type",
                table: "paymentHistory");

            migrationBuilder.AlterColumn<int>(
                name: "phone_no",
                table: "ProvidersDetails",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "subscription_id",
                table: "paymentHistory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Menuid",
                table: "order",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_order_Menuid",
                table: "order",
                column: "Menuid");

            migrationBuilder.AddForeignKey(
                name: "FK_order_menus_Menuid",
                table: "order",
                column: "Menuid",
                principalTable: "menus",
                principalColumn: "id");

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
