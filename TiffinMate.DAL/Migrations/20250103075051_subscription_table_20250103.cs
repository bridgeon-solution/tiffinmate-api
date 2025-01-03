using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class subscription_table_20250103 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider_id = table.Column<Guid>(type: "uuid", nullable: false),
                    menu_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<string>(type: "text", nullable: false),
                    cancelled_at = table.Column<string>(type: "text", nullable: false),
                    total_price = table.Column<decimal>(type: "numeric", nullable: false),
                    order_string = table.Column<string>(type: "text", nullable: true),
                    transaction_id = table.Column<string>(type: "text", nullable: true),
                    payment_status = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    Menuid = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.id);
                    table.ForeignKey(
                        name: "FK_Subscription_Providers_provider_id",
                        column: x => x.provider_id,
                        principalTable: "Providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscription_menus_Menuid",
                        column: x => x.Menuid,
                        principalTable: "menus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Subscription_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionDetails",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    subscription_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    ph_no = table.Column<string>(type: "text", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_SubscriptionDetails_Subscription_subscription_id",
                        column: x => x.subscription_id,
                        principalTable: "Subscription",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_Menuid",
                table: "Subscription",
                column: "Menuid");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_provider_id",
                table: "Subscription",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_user_id",
                table: "Subscription",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionDetails_subscription_id",
                table: "SubscriptionDetails",
                column: "subscription_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionDetails");

            migrationBuilder.DropTable(
                name: "Subscription");
        }
    }
}
