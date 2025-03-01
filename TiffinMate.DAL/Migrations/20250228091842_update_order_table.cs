using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update_order_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_menus_Menuid",
                table: "order");

            migrationBuilder.DropIndex(
                name: "IX_order_Menuid",
                table: "order");

            migrationBuilder.DropColumn(
                name: "Menuid",
                table: "order");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_menus_menu_id",
                table: "order");

            migrationBuilder.DropIndex(
                name: "IX_order_menu_id",
                table: "order");

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
        }
    }
}
