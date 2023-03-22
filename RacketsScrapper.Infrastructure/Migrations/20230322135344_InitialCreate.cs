using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RacketsScrapper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rackets",
                columns: table => new
                {
                    RacketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prezzo = table.Column<double>(type: "float", nullable: false),
                    Sesso = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColoreUno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColoreDue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profilo = table.Column<int>(type: "int", nullable: false),
                    Lunghezza = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Peso = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroArticolo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PuntoDiEquilibrio = table.Column<int>(type: "int", nullable: false),
                    TipoDiGioco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoDiProdotto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telaio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nucleo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LivelloDiGioco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Forma = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Eta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bilanciamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Anno = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rackets", x => x.RacketId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rackets");
        }
    }
}
