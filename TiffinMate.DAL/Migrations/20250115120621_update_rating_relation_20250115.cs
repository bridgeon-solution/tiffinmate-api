using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update_rating_relation_20250115 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Providers_providerid",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_users_userid",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_providerid",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_userid",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "providerid",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "userid",
                table: "Ratings");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_provider_id",
                table: "Ratings",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_user_id",
                table: "Ratings",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Providers_provider_id",
                table: "Ratings",
                column: "provider_id",
                principalTable: "Providers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_users_user_id",
                table: "Ratings",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Providers_provider_id",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_users_user_id",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_provider_id",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_user_id",
                table: "Ratings");

            migrationBuilder.AddColumn<Guid>(
                name: "providerid",
                table: "Ratings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "userid",
                table: "Ratings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_providerid",
                table: "Ratings",
                column: "providerid");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_userid",
                table: "Ratings",
                column: "userid");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Providers_providerid",
                table: "Ratings",
                column: "providerid",
                principalTable: "Providers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_users_userid",
                table: "Ratings",
                column: "userid",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
