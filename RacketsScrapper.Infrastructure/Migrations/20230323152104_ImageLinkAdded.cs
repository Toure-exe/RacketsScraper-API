﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RacketsScrapper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImageLinkAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Rackets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Rackets");
        }
    }
}
