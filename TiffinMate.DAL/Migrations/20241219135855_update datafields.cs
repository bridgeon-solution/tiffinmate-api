using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updatedatafields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Categories_categoryid",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Providers_providerid",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProvidersDetails_Providers_ProviderId",
                table: "ProvidersDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Providers_ProviderId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_users_UserId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reviews",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ProviderId",
                table: "Reviews",
                newName: "provider_id");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                newName: "IX_Reviews_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ProviderId",
                table: "Reviews",
                newName: "IX_Reviews_provider_id");

            migrationBuilder.RenameColumn(
                name: "ProviderId",
                table: "ProvidersDetails",
                newName: "provider_id");

            migrationBuilder.RenameIndex(
                name: "IX_ProvidersDetails_ProviderId",
                table: "ProvidersDetails",
                newName: "IX_ProvidersDetails_provider_id");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Providers",
                newName: "user_name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FoodItems",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "providerid",
                table: "FoodItems",
                newName: "provider_id");

            migrationBuilder.RenameColumn(
                name: "foodname",
                table: "FoodItems",
                newName: "food_name");

            migrationBuilder.RenameColumn(
                name: "categoryid",
                table: "FoodItems",
                newName: "category_id");

            migrationBuilder.RenameIndex(
                name: "IX_FoodItems_providerid",
                table: "FoodItems",
                newName: "IX_FoodItems_provider_id");

            migrationBuilder.RenameIndex(
                name: "IX_FoodItems_categoryid",
                table: "FoodItems",
                newName: "IX_FoodItems_category_id");

            migrationBuilder.RenameColumn(
                name: "categoryname",
                table: "Categories",
                newName: "category_name");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Admins",
                newName: "user_name");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Categories_category_id",
                table: "FoodItems",
                column: "category_id",
                principalTable: "Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Providers_provider_id",
                table: "FoodItems",
                column: "provider_id",
                principalTable: "Providers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProvidersDetails_Providers_provider_id",
                table: "ProvidersDetails",
                column: "provider_id",
                principalTable: "Providers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Providers_provider_id",
                table: "Reviews",
                column: "provider_id",
                principalTable: "Providers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_users_user_id",
                table: "Reviews",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Categories_category_id",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Providers_provider_id",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProvidersDetails_Providers_provider_id",
                table: "ProvidersDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Providers_provider_id",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_users_user_id",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Reviews",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "provider_id",
                table: "Reviews",
                newName: "ProviderId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_user_id",
                table: "Reviews",
                newName: "IX_Reviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_provider_id",
                table: "Reviews",
                newName: "IX_Reviews_ProviderId");

            migrationBuilder.RenameColumn(
                name: "provider_id",
                table: "ProvidersDetails",
                newName: "ProviderId");

            migrationBuilder.RenameIndex(
                name: "IX_ProvidersDetails_provider_id",
                table: "ProvidersDetails",
                newName: "IX_ProvidersDetails_ProviderId");

            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "Providers",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "FoodItems",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "provider_id",
                table: "FoodItems",
                newName: "providerid");

            migrationBuilder.RenameColumn(
                name: "food_name",
                table: "FoodItems",
                newName: "foodname");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "FoodItems",
                newName: "categoryid");

            migrationBuilder.RenameIndex(
                name: "IX_FoodItems_provider_id",
                table: "FoodItems",
                newName: "IX_FoodItems_providerid");

            migrationBuilder.RenameIndex(
                name: "IX_FoodItems_category_id",
                table: "FoodItems",
                newName: "IX_FoodItems_categoryid");

            migrationBuilder.RenameColumn(
                name: "category_name",
                table: "Categories",
                newName: "categoryname");

            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "Admins",
                newName: "username");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Categories_categoryid",
                table: "FoodItems",
                column: "categoryid",
                principalTable: "Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Providers_providerid",
                table: "FoodItems",
                column: "providerid",
                principalTable: "Providers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProvidersDetails_Providers_ProviderId",
                table: "ProvidersDetails",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Providers_ProviderId",
                table: "Reviews",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_users_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
