using RacketsScrapper.Domain;
using RacketsScrapper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacketsScrapper.Application
{
    public class RacketCrudService : IRacketCrudService
    {
        private readonly IRacketsRepository _racketsRepository;

        public RacketCrudService(IRacketsRepository racketsRepository)
        {
            _racketsRepository = racketsRepository;
        }
        public bool DeleteAllRackets()
        {
           return _racketsRepository.DeleteAllRackets();
        }

        public IEnumerable<Racket> GetTenRackets()
        {
           return  _racketsRepository.GetTenRackets();
        }

        public bool DeleteRacketbyId(int id)
        {
           Racket? toDelete = _racketsRepository.GetRacketById(id);
           return (toDelete != null) && _racketsRepository.DeleteRacket(toDelete);
        }

        public ResponseFilterObject GetAllRackets(int currentPage)
        {
            return _racketsRepository.GetAllRackets(currentPage);
        }

        public Racket? GetRacketById(int id)
        {
            return _racketsRepository.GetRacketById(id);
        }

        public bool ModifyRacket(Racket racket)
        {
            return _racketsRepository.UpdateRacket(racket);
        }

        public ResponseFilterObject GetRacketByName(string name, int page)
        {
           return _racketsRepository.GetRacketByName(name,page);
        }

        public IEnumerable<Racket> OrderByPriceAsc(IEnumerable<Racket> values)
        {
            return _racketsRepository.OrderByPriceAsc(values);
        }

        public IEnumerable<Racket> OrderByPriceDesc(IEnumerable<Racket> values)
        {
            return _racketsRepository.OrderByPriceDesc(values);
        }

        public ResponseFilterObject GetAllRacketsWithFilter(RequestFilterObject request, int page)
        {
                return _racketsRepository.GetAllRacketsWithFilter(request, page); 
        }

        public bool InsertRacket(Racket racket)
        {
            return _racketsRepository.InsertRacket(racket);
        }
    }
}
