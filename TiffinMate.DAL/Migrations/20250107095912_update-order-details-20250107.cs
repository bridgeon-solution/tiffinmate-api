using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateorderdetails20250107 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_category_id",
                table: "orderDetails",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_Categories_category_id",
                table: "orderDetails",
                column: "category_id",
                principalTable: "Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_Categories_category_id",
                table: "orderDetails");

            migrationBuilder.DropIndex(
                name: "IX_orderDetails_category_id",
                table: "orderDetails");
        }
    }
}
