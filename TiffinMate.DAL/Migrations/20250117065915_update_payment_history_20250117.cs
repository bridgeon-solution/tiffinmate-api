using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update_payment_history_20250117 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "paymentHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "paymentHistory",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "paymentHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_paymentHistory_user_id",
                table: "paymentHistory",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_paymentHistory_users_user_id",
                table: "paymentHistory",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_paymentHistory_users_user_id",
                table: "paymentHistory");

            migrationBuilder.DropIndex(
                name: "IX_paymentHistory_user_id",
                table: "paymentHistory");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "paymentHistory");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "paymentHistory");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "paymentHistory");
        }
    }
}
