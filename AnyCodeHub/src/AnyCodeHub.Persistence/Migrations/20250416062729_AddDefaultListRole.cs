using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AnyCodeHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultListRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
