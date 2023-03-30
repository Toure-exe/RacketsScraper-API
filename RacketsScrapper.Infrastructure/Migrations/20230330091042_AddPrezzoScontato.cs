using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RacketsScrapper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPrezzoScontato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PrezzoScontato",
                table: "Rackets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrezzoScontato",
                table: "Rackets");
        }
    }
}
