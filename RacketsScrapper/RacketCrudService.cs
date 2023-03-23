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

        public bool DeleteRacketbyId(int id)
        {
           Racket toDelete = _racketsRepository.GetRacketById(id);
            return _racketsRepository.DeleteRacket(toDelete);
        }

        public Racket GetRacketById(int id)
        {
            return _racketsRepository.GetRacketById(id);
        }

        public bool ModifyRacket(Racket racket)
        {
            return _racketsRepository.UpdateRacket(racket);
        }
    }
}
