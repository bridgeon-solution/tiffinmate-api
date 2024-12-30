using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class add_order_orderdetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider_id = table.Column<Guid>(type: "uuid", nullable: false),
                    menu_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    total_price = table.Column<decimal>(type: "numeric", nullable: false),
                    Menuid = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_Providers_provider_id",
                        column: x => x.provider_id,
                        principalTable: "Providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_menus_Menuid",
                        column: x => x.Menuid,
                        principalTable: "menus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_order_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesOrder",
                columns: table => new
                {
                    categoryid = table.Column<Guid>(type: "uuid", nullable: false),
                    orderid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesOrder", x => new { x.categoryid, x.orderid });
                    table.ForeignKey(
                        name: "FK_CategoriesOrder_Categories_categoryid",
                        column: x => x.categoryid,
                        principalTable: "Categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriesOrder_order_orderid",
                        column: x => x.orderid,
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderCategory",
                columns: table => new
                {
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCategory", x => new { x.order_id, x.category_id });
                    table.ForeignKey(
                        name: "FK_OrderCategory_Categories_category_id",
                        column: x => x.category_id,
                        principalTable: "Categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderCategory_order_order_id",
                        column: x => x.order_id,
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orderDetails",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    fooditem_name = table.Column<string>(type: "text", nullable: false),
                    fooditem_image = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_orderDetails_order_order_id",
                        column: x => x.order_id,
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesOrder_orderid",
                table: "CategoriesOrder",
                column: "orderid");

            migrationBuilder.CreateIndex(
                name: "IX_order_Menuid",
                table: "order",
                column: "Menuid");

            migrationBuilder.CreateIndex(
                name: "IX_order_provider_id",
                table: "order",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_user_id",
                table: "order",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCategory_category_id",
                table: "OrderCategory",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_order_id",
                table: "orderDetails",
                column: "order_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriesOrder");

            migrationBuilder.DropTable(
                name: "OrderCategory");

            migrationBuilder.DropTable(
                name: "orderDetails");

            migrationBuilder.DropTable(
                name: "order");
        }
    }
}
