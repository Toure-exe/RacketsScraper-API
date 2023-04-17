using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RacketsScrapper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0800a606-3642-4b19-a6ff-17da8b28eae8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f640d84-db2b-4590-949d-2cae60fb5630");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "96e74937-be5d-4dda-af95-605b2569e889");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "597e07cc-db38-42c8-a642-01c38c302f25", "f6ede661-3ce9-45ae-b6ab-4d640a151ad1", "Admin", "ADMIN" },
                    { "c64aad23-f4a0-4db3-823a-3280143dc122", "e78f545f-2a7a-42dd-a970-34f5496537ed", "User", "USER" },
                    { "e6e98401-e9fa-4e36-b92d-790f74925a87", "8d87be9c-a89f-4a85-9a86-0299f04c2cef", "Super_User", "SUPER_USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "597e07cc-db38-42c8-a642-01c38c302f25");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c64aad23-f4a0-4db3-823a-3280143dc122");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6e98401-e9fa-4e36-b92d-790f74925a87");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0800a606-3642-4b19-a6ff-17da8b28eae8", "2e9a1f96-a99e-4671-9fa4-6ea6792d2255", "Utente con privileggi", "UTENTE CON PRIVILEGGI" },
                    { "1f640d84-db2b-4590-949d-2cae60fb5630", "b4c3886c-7007-4440-9e4d-6ff5816ec27d", "Amministratore", "AMMINISTRATORE" },
                    { "96e74937-be5d-4dda-af95-605b2569e889", "1cb529cc-2226-4511-9a8d-65ca18cf39e7", "Utente Normale", "UTENTE NORMALE" }
                });
        }
    }
}
