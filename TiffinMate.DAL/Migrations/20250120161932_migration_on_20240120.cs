using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class migration_on_20240120 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subscriptions_menus_Menuid",
                table: "subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_subscriptions_Menuid",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "Menuid",
                table: "subscriptions");

            migrationBuilder.CreateIndex(
                name: "IX_subscriptions_menu_id",
                table: "subscriptions",
                column: "menu_id");

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptions_menus_menu_id",
                table: "subscriptions",
                column: "menu_id",
                principalTable: "menus",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subscriptions_menus_menu_id",
                table: "subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_subscriptions_menu_id",
                table: "subscriptions");

            migrationBuilder.AddColumn<Guid>(
                name: "Menuid",
                table: "subscriptions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_subscriptions_Menuid",
                table: "subscriptions",
                column: "Menuid");

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptions_menus_Menuid",
                table: "subscriptions",
                column: "Menuid",
                principalTable: "menus",
                principalColumn: "id");
        }
    }
}
