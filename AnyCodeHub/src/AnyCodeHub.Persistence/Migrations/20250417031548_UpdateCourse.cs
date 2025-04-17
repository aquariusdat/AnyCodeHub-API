using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AnyCodeHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("1b7383e9-3aed-45cb-ac60-6797306f506f"));

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("2c9d6fe9-cd20-4704-94e0-3c0e1cbb37fd"));

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("38c46b9c-3aeb-4e06-a31e-41f573cf0878"));

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("47d6dd1d-ef7b-4a5a-b343-137b70cde1e9"));

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName", "RoleCode" },
                values: new object[,]
                {
                    { new Guid("1b7383e9-3aed-45cb-ac60-6797306f506f"), null, "USER", "USER", "USER", "USER" },
                    { new Guid("2c9d6fe9-cd20-4704-94e0-3c0e1cbb37fd"), null, "ADMIN", "ADMIN", "ADMIN", "ADMIN" },
                    { new Guid("38c46b9c-3aeb-4e06-a31e-41f573cf0878"), null, "MODERATOR", "MODERATOR", "MODERATOR", "MODERATOR" },
                    { new Guid("47d6dd1d-ef7b-4a5a-b343-137b70cde1e9"), null, "GUEST", "GUEST", "GUEST", "GUEST" }
                });
        }
    }
}
