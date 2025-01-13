using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updatesubscriptiontable20250108 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_subscriptionDetails_category_id",
                table: "subscriptionDetails",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptionDetails_Categories_category_id",
                table: "subscriptionDetails",
                column: "category_id",
                principalTable: "Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subscriptionDetails_Categories_category_id",
                table: "subscriptionDetails");

            migrationBuilder.DropIndex(
                name: "IX_subscriptionDetails_category_id",
                table: "subscriptionDetails");
        }
    }
}
