using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updatesubscription_table_20250103 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Providers_provider_id",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_menus_Menuid",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_users_user_id",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionDetails_Subscription_subscription_id",
                table: "SubscriptionDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubscriptionDetails",
                table: "SubscriptionDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription");

            migrationBuilder.RenameTable(
                name: "SubscriptionDetails",
                newName: "subscriptionDetails");

            migrationBuilder.RenameTable(
                name: "Subscription",
                newName: "subscriptions");

            migrationBuilder.RenameIndex(
                name: "IX_SubscriptionDetails_subscription_id",
                table: "subscriptionDetails",
                newName: "IX_subscriptionDetails_subscription_id");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_user_id",
                table: "subscriptions",
                newName: "IX_subscriptions_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_provider_id",
                table: "subscriptions",
                newName: "IX_subscriptions_provider_id");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_Menuid",
                table: "subscriptions",
                newName: "IX_subscriptions_Menuid");

            migrationBuilder.AlterColumn<string>(
                name: "cancelled_at",
                table: "subscriptions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_subscriptionDetails",
                table: "subscriptionDetails",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_subscriptions",
                table: "subscriptions",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptionDetails_subscriptions_subscription_id",
                table: "subscriptionDetails",
                column: "subscription_id",
                principalTable: "subscriptions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptions_Providers_provider_id",
                table: "subscriptions",
                column: "provider_id",
                principalTable: "Providers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptions_menus_Menuid",
                table: "subscriptions",
                column: "Menuid",
                principalTable: "menus",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptions_users_user_id",
                table: "subscriptions",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subscriptionDetails_subscriptions_subscription_id",
                table: "subscriptionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_subscriptions_Providers_provider_id",
                table: "subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_subscriptions_menus_Menuid",
                table: "subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_subscriptions_users_user_id",
                table: "subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_subscriptionDetails",
                table: "subscriptionDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_subscriptions",
                table: "subscriptions");

            migrationBuilder.RenameTable(
                name: "subscriptionDetails",
                newName: "SubscriptionDetails");

            migrationBuilder.RenameTable(
                name: "subscriptions",
                newName: "Subscription");

            migrationBuilder.RenameIndex(
                name: "IX_subscriptionDetails_subscription_id",
                table: "SubscriptionDetails",
                newName: "IX_SubscriptionDetails_subscription_id");

            migrationBuilder.RenameIndex(
                name: "IX_subscriptions_user_id",
                table: "Subscription",
                newName: "IX_Subscription_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_subscriptions_provider_id",
                table: "Subscription",
                newName: "IX_Subscription_provider_id");

            migrationBuilder.RenameIndex(
                name: "IX_subscriptions_Menuid",
                table: "Subscription",
                newName: "IX_Subscription_Menuid");

            migrationBuilder.AlterColumn<string>(
                name: "cancelled_at",
                table: "Subscription",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubscriptionDetails",
                table: "SubscriptionDetails",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Providers_provider_id",
                table: "Subscription",
                column: "provider_id",
                principalTable: "Providers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_menus_Menuid",
                table: "Subscription",
                column: "Menuid",
                principalTable: "menus",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_users_user_id",
                table: "Subscription",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionDetails_Subscription_subscription_id",
                table: "SubscriptionDetails",
                column: "subscription_id",
                principalTable: "Subscription",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
