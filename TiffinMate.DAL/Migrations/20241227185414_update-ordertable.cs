using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateordertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderCategory_Categories_category_id",
                table: "OrderCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderCategory_order_order_id",
                table: "OrderCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderCategory",
                table: "OrderCategory");

            migrationBuilder.RenameTable(
                name: "OrderCategory",
                newName: "orderCategory");

            migrationBuilder.RenameIndex(
                name: "IX_OrderCategory_category_id",
                table: "orderCategory",
                newName: "IX_orderCategory_category_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_orderCategory",
                table: "orderCategory",
                columns: new[] { "order_id", "category_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_orderCategory_Categories_category_id",
                table: "orderCategory",
                column: "category_id",
                principalTable: "Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orderCategory_order_order_id",
                table: "orderCategory",
                column: "order_id",
                principalTable: "order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderCategory_Categories_category_id",
                table: "orderCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_orderCategory_order_order_id",
                table: "orderCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orderCategory",
                table: "orderCategory");

            migrationBuilder.RenameTable(
                name: "orderCategory",
                newName: "OrderCategory");

            migrationBuilder.RenameIndex(
                name: "IX_orderCategory_category_id",
                table: "OrderCategory",
                newName: "IX_OrderCategory_category_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderCategory",
                table: "OrderCategory",
                columns: new[] { "order_id", "category_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderCategory_Categories_category_id",
                table: "OrderCategory",
                column: "category_id",
                principalTable: "Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderCategory_order_order_id",
                table: "OrderCategory",
                column: "order_id",
                principalTable: "order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
