using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AnyCodeHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeDatabase_V11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseComments_Comments_CommentId",
                table: "CourseComments");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonComments_Comments_CommentId",
                table: "LessonComments");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_LessonComments_CommentId",
                table: "LessonComments");

            migrationBuilder.DropIndex(
                name: "IX_CourseComments_CommentId",
                table: "CourseComments");

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("09c69b19-c5fd-4563-aeac-cff84ed5e387"));

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("693079c7-1a93-43d0-b0c1-68671c70d6c2"));

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("73d98e79-a741-4b8e-bf22-288fa1007e14"));

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("f735e811-9e83-4a1a-8ee9-0028e2fe2e18"));

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "LessonComments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "CourseComments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName", "RoleCode" },
                values: new object[,]
                {
                    { new Guid("02fbfed6-c5ca-41b9-bcc1-8e45e82ca595"), null, "MODERATOR", "MODERATOR", "MODERATOR", "MODERATOR" },
                    { new Guid("1867f51f-cbdc-45e1-ba25-8d5cd4ebbdde"), null, "ADMIN", "ADMIN", "ADMIN", "ADMIN" },
                    { new Guid("2209cf27-18b4-4b00-b00e-e00dfd5d025f"), null, "GUEST", "GUEST", "GUEST", "GUEST" },
                    { new Guid("80e62f7a-e8b5-4a89-a7ff-10aebb6dd6bb"), null, "USER", "USER", "USER", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("02fbfed6-c5ca-41b9-bcc1-8e45e82ca595"));

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("1867f51f-cbdc-45e1-ba25-8d5cd4ebbdde"));

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("2209cf27-18b4-4b00-b00e-e00dfd5d025f"));

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("80e62f7a-e8b5-4a89-a7ff-10aebb6dd6bb"));

            migrationBuilder.DropColumn(
                name: "Content",
                table: "LessonComments");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "CourseComments");

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName", "RoleCode" },
                values: new object[,]
                {
                    { new Guid("09c69b19-c5fd-4563-aeac-cff84ed5e387"), null, "GUEST", "GUEST", "GUEST", "GUEST" },
                    { new Guid("693079c7-1a93-43d0-b0c1-68671c70d6c2"), null, "USER", "USER", "USER", "USER" },
                    { new Guid("73d98e79-a741-4b8e-bf22-288fa1007e14"), null, "ADMIN", "ADMIN", "ADMIN", "ADMIN" },
                    { new Guid("f735e811-9e83-4a1a-8ee9-0028e2fe2e18"), null, "MODERATOR", "MODERATOR", "MODERATOR", "MODERATOR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonComments_CommentId",
                table: "LessonComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseComments_CommentId",
                table: "CourseComments",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseComments_Comments_CommentId",
                table: "CourseComments",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonComments_Comments_CommentId",
                table: "LessonComments",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
