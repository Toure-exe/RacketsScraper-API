using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RacketsScrapper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyPrezzoScontato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrezzoScontato",
                table: "Rackets",
                newName: "VecchioPrezzo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VecchioPrezzo",
                table: "Rackets",
                newName: "PrezzoScontato");
        }
    }
}
