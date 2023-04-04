using RacketsScrapper.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacketsScrapper.Infrastructure
{
    public interface IRacketsRepository
    {
        public bool InsertRacket(Racket racket);

        public Racket? GetRacketById(int id);

        public bool DeleteAllRackets();

        public IEnumerable<Racket> GetTenRackets();

        public bool DeleteRacket(Racket racket);

        public bool UpdateRacket(Racket racket);

        public ResponseObject GetAllRackets(int currentPage);

        IEnumerable<Racket> GetRacketByName(string name, int page);
        IEnumerable<Racket> OrderByPriceAsc(IEnumerable<Racket> values);
        IEnumerable<Racket> OrderByPriceDesc(IEnumerable<Racket> values);

        public ResponseObject GetAllRacketsWithFilter(RequestObject request, int page);

        public ResponseObject IndexPage(int page);
    }
}
