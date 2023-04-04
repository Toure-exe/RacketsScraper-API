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

        public ResponseObject GetAllRackets(int currentPage);

        public IEnumerable<Racket> GetTenRackets();
        IEnumerable<Racket> GetRacketByName(string name, int page);
        IEnumerable<Racket> OrderByPriceAsc(IEnumerable<Racket> values);
        IEnumerable<Racket> OrderByPriceDesc(IEnumerable<Racket> values);

        public ResponseObject GetAllRacketsWithFilter(RequestObject request, int page);

       // public ResponseObject GetRacketsByPage(int page);
    }
}
