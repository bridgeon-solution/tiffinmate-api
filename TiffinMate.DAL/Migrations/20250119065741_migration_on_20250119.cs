using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class migration_on_20250119 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    refresh_token = table.Column<string>(type: "text", nullable: false),
                    refreshtoken_expiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ApiLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HttpMethod = table.Column<string>(type: "text", nullable: false),
                    RequestPath = table.Column<string>(type: "text", nullable: false),
                    QueryString = table.Column<string>(type: "text", nullable: false),
                    RequestBody = table.Column<string>(type: "text", nullable: false),
                    ResponseStatusCode = table.Column<int>(type: "integer", nullable: false),
                    ResponseBody = table.Column<string>(type: "text", nullable: false),
                    LoggedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeTaken = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    recipient_type = table.Column<string>(type: "text", nullable: false),
                    recipient_id = table.Column<string>(type: "text", nullable: true),
                    notification_type = table.Column<string>(type: "text", nullable: false),
                    isread = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    email = table.Column<string>(type: "text", nullable: true),
                    certificate = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    role = table.Column<string>(type: "text", nullable: false, defaultValue: "provider"),
                    is_blocked = table.Column<bool>(type: "boolean", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: true),
                    verification_status = table.Column<string>(type: "text", nullable: false, defaultValue: "False"),
                    refresh_token = table.Column<string>(type: "text", nullable: false),
                    refreshtoken_expiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false, defaultValue: "user"),
                    subscription_status = table.Column<bool>(type: "boolean", nullable: false),
                    address = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true),
                    is_blocked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    refresh_token = table.Column<string>(type: "text", nullable: false),
                    refreshtoken_expiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "menus",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    image = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    is_available = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    monthly_plan_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menus", x => x.id);
                    table.ForeignKey(
                        name: "FK_menus_Providers_provider_id",
                        column: x => x.provider_id,
                        principalTable: "Providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProvidersDetails",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    provider_id = table.Column<Guid>(type: "uuid", nullable: false),
                    resturent_name = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    phone_no = table.Column<int>(type: "integer", nullable: false),
                    location = table.Column<string>(type: "text", nullable: false),
                    logo = table.Column<string>(type: "text", nullable: false),
                    about = table.Column<string>(type: "text", nullable: false),
                    image = table.Column<string>(type: "text", nullable: false),
                    account_no = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvidersDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProvidersDetails_Providers_provider_id",
                        column: x => x.provider_id,
                        principalTable: "Providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.id);
                    table.ForeignKey(
                        name: "FK_Ratings_Providers_provider_id",
                        column: x => x.provider_id,
                        principalTable: "Providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    review = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.id);
                    table.ForeignKey(
                        name: "FK_Reviews_Providers_provider_id",
                        column: x => x.provider_id,
                        principalTable: "Providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodItems",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    food_name = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    menu_id = table.Column<Guid>(type: "uuid", nullable: false),
                    day = table.Column<string>(type: "text", nullable: false),
                    image = table.Column<string>(type: "text", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItems", x => x.id);
                    table.ForeignKey(
                        name: "FK_FoodItems_Categories_category_id",
                        column: x => x.category_id,
                        principalTable: "Categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodItems_Providers_provider_id",
                        column: x => x.provider_id,
                        principalTable: "Providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodItems_menus_menu_id",
                        column: x => x.menu_id,
                        principalTable: "menus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider_id = table.Column<Guid>(type: "uuid", nullable: false),
                    menu_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<string>(type: "text", nullable: false),
                    total_price = table.Column<decimal>(type: "numeric", nullable: false),
                    order_string = table.Column<string>(type: "text", nullable: true),
                    transaction_id = table.Column<string>(type: "text", nullable: true),
                    order_status = table.Column<int>(type: "integer", nullable: false),
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
                name: "subscriptions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider_id = table.Column<Guid>(type: "uuid", nullable: false),
                    menu_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<string>(type: "text", nullable: false),
                    cancelled_at = table.Column<string>(type: "text", nullable: true),
                    total_price = table.Column<decimal>(type: "numeric", nullable: false),
                    order_string = table.Column<string>(type: "text", nullable: true),
                    transaction_id = table.Column<string>(type: "text", nullable: true),
                    order_status = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    Menuid = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscriptions", x => x.id);
                    table.ForeignKey(
                        name: "FK_subscriptions_Providers_provider_id",
                        column: x => x.provider_id,
                        principalTable: "Providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_subscriptions_menus_Menuid",
                        column: x => x.Menuid,
                        principalTable: "menus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_subscriptions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
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
                    ph_no = table.Column<string>(type: "text", nullable: false),
                    fooditem_name = table.Column<string>(type: "text", nullable: false),
                    fooditem_image = table.Column<string>(type: "text", nullable: false),
                    fooditem_price = table.Column<decimal>(type: "numeric", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_orderDetails_Categories_category_id",
                        column: x => x.category_id,
                        principalTable: "Categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orderDetails_order_order_id",
                        column: x => x.order_id,
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "paymentHistory",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    payment_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_paid = table.Column<bool>(type: "boolean", nullable: false),
                    subscription_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentHistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_paymentHistory_subscriptions_subscription_id",
                        column: x => x.subscription_id,
                        principalTable: "subscriptions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_paymentHistory_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subscriptionDetails",
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
                    table.PrimaryKey("PK_subscriptionDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_subscriptionDetails_Categories_category_id",
                        column: x => x.category_id,
                        principalTable: "Categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_subscriptionDetails_subscriptions_subscription_id",
                        column: x => x.subscription_id,
                        principalTable: "subscriptions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_category_id",
                table: "FoodItems",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_menu_id",
                table: "FoodItems",
                column: "menu_id");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_provider_id",
                table: "FoodItems",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_menus_provider_id",
                table: "menus",
                column: "provider_id");

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
                name: "IX_orderDetails_category_id",
                table: "orderDetails",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_order_id",
                table: "orderDetails",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_paymentHistory_subscription_id",
                table: "paymentHistory",
                column: "subscription_id");

            migrationBuilder.CreateIndex(
                name: "IX_paymentHistory_user_id",
                table: "paymentHistory",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProvidersDetails_provider_id",
                table: "ProvidersDetails",
                column: "provider_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_provider_id",
                table: "Ratings",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_user_id",
                table: "Ratings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_provider_id",
                table: "Reviews",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_user_id",
                table: "Reviews",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_subscriptionDetails_category_id",
                table: "subscriptionDetails",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_subscriptionDetails_subscription_id",
                table: "subscriptionDetails",
                column: "subscription_id");

            migrationBuilder.CreateIndex(
                name: "IX_subscriptions_Menuid",
                table: "subscriptions",
                column: "Menuid");

            migrationBuilder.CreateIndex(
                name: "IX_subscriptions_provider_id",
                table: "subscriptions",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_subscriptions_user_id",
                table: "subscriptions",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "ApiLogs");

            migrationBuilder.DropTable(
                name: "FoodItems");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "orderDetails");

            migrationBuilder.DropTable(
                name: "paymentHistory");

            migrationBuilder.DropTable(
                name: "ProvidersDetails");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "subscriptionDetails");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "subscriptions");

            migrationBuilder.DropTable(
                name: "menus");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "Providers");
        }
    }
}
