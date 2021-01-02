using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HardwareAffinity.Data.Migrations
{
    public partial class MakeCartProductsDeletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CartProducts",
                table: "CartProducts");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CartProducts",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "CartProducts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "CartProducts",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CartProducts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "CartProducts",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartProducts",
                table: "CartProducts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CartProducts_CartId",
                table: "CartProducts",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartProducts_IsDeleted",
                table: "CartProducts",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CartProducts",
                table: "CartProducts");

            migrationBuilder.DropIndex(
                name: "IX_CartProducts_CartId",
                table: "CartProducts");

            migrationBuilder.DropIndex(
                name: "IX_CartProducts_IsDeleted",
                table: "CartProducts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CartProducts");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "CartProducts");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "CartProducts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CartProducts");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "CartProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartProducts",
                table: "CartProducts",
                columns: new[] { "CartId", "ProductId" });
        }
    }
}
