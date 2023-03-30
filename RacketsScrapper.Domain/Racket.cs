using System.ComponentModel.DataAnnotations;

namespace RacketsScrapper.Domain
{
    public class Racket
    {
        public int RacketId { get; set; }

        public double Prezzo { get; set; }

        public double VecchioPrezzo { get; set; }

        public string? Marca { get; set; }

        public string? Modello { get; set; }

        public string? Sesso { get;set; }

        public string? ImageLink { get; set; }

        public string? ColoreUno { get; set; }

        public string? ColoreDue { get; set; }

        public int Profilo { get; set; }

        public string? Lunghezza { get; set; }

        public string? Peso { get; set; }

        public string? NumeroArticolo { get; set; }

        public int PuntoDiEquilibrio { get; set; }

        public string? TipoDiGioco { get; set; }

        public string? Url { get; set; }

        public string? TipoDiProdotto { get; set; }

        public string? Telaio { get; set; }

        public string? Nucleo { get; set; }

        public string? LivelloDiGioco { get; set; }

        public string? Forma { get; set; }

        public string? Eta { get; set;}

        public string? Bilanciamento { get; set;}

        public string? Anno { get; set; }


    }
}