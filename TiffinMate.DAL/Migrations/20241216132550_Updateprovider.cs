using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Updateprovider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_certificate_verified",
                table: "Providers");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiryDate",
                table: "Providers",
                newName: "refreshtoken_expiryDate");

            migrationBuilder.AddColumn<string>(
                name: "verification_status",
                table: "Providers",
                type: "text",
                nullable: false,
                defaultValue: "False");

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    review = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => new { x.ProviderId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Reviews_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropColumn(
                name: "verification_status",
                table: "Providers");

            migrationBuilder.RenameColumn(
                name: "refreshtoken_expiryDate",
                table: "Providers",
                newName: "RefreshTokenExpiryDate");

            migrationBuilder.AddColumn<bool>(
                name: "is_certificate_verified",
                table: "Providers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
