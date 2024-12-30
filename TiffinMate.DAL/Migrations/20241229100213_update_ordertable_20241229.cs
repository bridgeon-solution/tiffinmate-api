using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update_ordertable_20241229 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoriesOrder_Categories_categoryid",
                table: "CategoriesOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoriesOrder_order_orderid",
                table: "CategoriesOrder");

            migrationBuilder.DropIndex(
                name: "IX_orderDetails_order_id",
                table: "orderDetails");

            migrationBuilder.RenameColumn(
                name: "orderid",
                table: "CategoriesOrder",
                newName: "order_id");

            migrationBuilder.RenameColumn(
                name: "categoryid",
                table: "CategoriesOrder",
                newName: "category_id");

            migrationBuilder.RenameIndex(
                name: "IX_CategoriesOrder_orderid",
                table: "CategoriesOrder",
                newName: "IX_CategoriesOrder_order_id");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_order_id",
                table: "orderDetails",
                column: "order_id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriesOrder_Categories_category_id",
                table: "CategoriesOrder",
                column: "category_id",
                principalTable: "Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriesOrder_order_order_id",
                table: "CategoriesOrder",
                column: "order_id",
                principalTable: "order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoriesOrder_Categories_category_id",
                table: "CategoriesOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoriesOrder_order_order_id",
                table: "CategoriesOrder");

            migrationBuilder.DropIndex(
                name: "IX_orderDetails_order_id",
                table: "orderDetails");

            migrationBuilder.RenameColumn(
                name: "order_id",
                table: "CategoriesOrder",
                newName: "orderid");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "CategoriesOrder",
                newName: "categoryid");

            migrationBuilder.RenameIndex(
                name: "IX_CategoriesOrder_order_id",
                table: "CategoriesOrder",
                newName: "IX_CategoriesOrder_orderid");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_order_id",
                table: "orderDetails",
                column: "order_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriesOrder_Categories_categoryid",
                table: "CategoriesOrder",
                column: "categoryid",
                principalTable: "Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriesOrder_order_orderid",
                table: "CategoriesOrder",
                column: "orderid",
                principalTable: "order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
