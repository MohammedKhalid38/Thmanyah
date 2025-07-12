using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPostCategoryTableAndLinkedWithPostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Locales_LocaleId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "LocaleId",
                table: "Posts",
                newName: "PostCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_LocaleId",
                table: "Posts",
                newName: "IX_Posts_PostCategoryId");

            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "Posts",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "PostCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Sort = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PublishedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsNeedDeleteApprove = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategories", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_PostCategories_PostCategoryId",
                table: "Posts",
                column: "PostCategoryId",
                principalTable: "PostCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_PostCategories_PostCategoryId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "PostCategories");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "PostCategoryId",
                table: "Posts",
                newName: "LocaleId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_PostCategoryId",
                table: "Posts",
                newName: "IX_Posts_LocaleId");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Posts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Locales_LocaleId",
                table: "Posts",
                column: "LocaleId",
                principalTable: "Locales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
