using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update_rating_table_20250115 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Providers_providerid",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_users_userid",
                table: "Rating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rating",
                table: "Rating");

            migrationBuilder.RenameTable(
                name: "Rating",
                newName: "Ratings");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_userid",
                table: "Ratings",
                newName: "IX_Ratings_userid");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_providerid",
                table: "Ratings",
                newName: "IX_Ratings_providerid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Providers_providerid",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_users_userid",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.RenameTable(
                name: "Ratings",
                newName: "Rating");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_userid",
                table: "Rating",
                newName: "IX_Rating_userid");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_providerid",
                table: "Rating",
                newName: "IX_Rating_providerid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rating",
                table: "Rating",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Providers_providerid",
                table: "Rating",
                column: "providerid",
                principalTable: "Providers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_users_userid",
                table: "Rating",
                column: "userid",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
