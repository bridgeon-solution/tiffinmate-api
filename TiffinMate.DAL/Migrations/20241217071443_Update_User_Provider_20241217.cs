using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Update_User_Provider_20241217 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_available",
                table: "FoodItems");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "ProvidersDetails",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ProvidersDetails",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Providers",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Providers",
                newName: "created_at");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "users",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "subscription_status",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "Reviews",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "Reviews",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "Reviews",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "Reviews",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "ProvidersDetails",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_blocked",
                table: "Providers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "Providers",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "users");

            migrationBuilder.DropColumn(
                name: "subscription_status",
                table: "users");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "id",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "ProvidersDetails");

            migrationBuilder.DropColumn(
                name: "is_blocked",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "Providers");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ProvidersDetails",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ProvidersDetails",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Providers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Providers",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_available",
                table: "FoodItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
