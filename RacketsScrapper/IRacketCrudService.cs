using RacketsScrapper.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacketsScrapper.Application
{
    public interface IRacketCrudService
    {
        public bool DeleteAllRackets();

        public bool DeleteRacketbyId(int id);

        public bool ModifyRacket(Racket racket);

        public Racket GetRacketById(int id);

        public ResponseFilterObject GetAllRackets(int currentPage);

        public IEnumerable<Racket> GetTenRackets();
        public ResponseFilterObject GetRacketByName(string name, int page);
        public IEnumerable<Racket> OrderByPriceAsc(IEnumerable<Racket> values);
        public IEnumerable<Racket> OrderByPriceDesc(IEnumerable<Racket> values);

        public ResponseFilterObject GetAllRacketsWithFilter(RequestFilterObject request, int page);

       public bool InsertRacket(Racket racket);
    }
}
