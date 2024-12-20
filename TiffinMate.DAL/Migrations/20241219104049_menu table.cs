using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class menutable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_veg",
                table: "FoodItems");

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "FoodItems",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "menu_id",
                table: "FoodItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "Categories",
                type: "boolean",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_menu_id",
                table: "FoodItems",
                column: "menu_id");

            migrationBuilder.CreateIndex(
                name: "IX_menus_provider_id",
                table: "menus",
                column: "provider_id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_menus_menu_id",
                table: "FoodItems",
                column: "menu_id",
                principalTable: "menus",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_menus_menu_id",
                table: "FoodItems");

            migrationBuilder.DropTable(
                name: "menus");

            migrationBuilder.DropIndex(
                name: "IX_FoodItems_menu_id",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "menu_id",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "Categories");

            migrationBuilder.AddColumn<bool>(
                name: "is_veg",
                table: "FoodItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
