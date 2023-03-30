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
           Racket toDelete = _racketsRepository.GetRacketById(id);
           return (toDelete != null) ?_racketsRepository.DeleteRacket(toDelete) : false;
        }

        public ResponseObject GetAllRackets(int currentPage)
        {
            return _racketsRepository.GetAllRackets(currentPage);
        }

        public Racket GetRacketById(int id)
        {
            return _racketsRepository.GetRacketById(id);
        }

        public bool ModifyRacket(Racket racket)
        {
            return _racketsRepository.UpdateRacket(racket);
        }

        public IEnumerable<Racket> GetRacketByName(string name)
        {
           return _racketsRepository.GetRacketByName(name);
        }

        public IEnumerable<Racket> OrderByPriceAsc(IEnumerable<Racket> values)
        {
            return _racketsRepository.OrderByPriceAsc(values);
        }

        public IEnumerable<Racket> OrderByPriceDesc(IEnumerable<Racket> values)
        {
            return _racketsRepository.OrderByPriceDesc(values);
        }
    }
}
