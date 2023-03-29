using Microsoft.EntityFrameworkCore;
using RacketsScrapper.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacketsScrapper.Infrastructure
{
    public class RacketRepository : IRacketsRepository
    {
        public readonly RacketDbContext _racketDbContext;
        private readonly ICacheService _cacheService;

        public RacketRepository(RacketDbContext racketDbContext, ICacheService cacheService)
        {
            _racketDbContext = racketDbContext;
            _cacheService = cacheService;
        }

        public bool DeleteAllRackets()
        {
            foreach (Racket racket in _racketDbContext.Rackets)
            {
                _racketDbContext.Rackets.Remove(racket);
                _cacheService.RemoveData($"{racket.RacketId}");
            }
            return _racketDbContext.SaveChanges() > 0;
        }

        public IEnumerable<Racket> GetTenRackets()
        {
            List<Racket> result = new List<Racket>();
            int i = 0;
            foreach (Racket racket in _racketDbContext.Rackets)
            {
                if (i >= 10)
                    break;
                else
                {
                    result.Add(racket);
                    i++;
                }
            }

            return result;
        }

        public bool DeleteRacket(Racket racket)
        {
            _cacheService.RemoveData($"{racket.RacketId}");
            _racketDbContext.Rackets.Remove(racket);
            return _racketDbContext.SaveChanges() > 0;
        }

        public IEnumerable<Racket> GetAllRackets()
        {
            return _racketDbContext.Rackets.ToList();
        }

        public Racket? GetRacketById(int id)
        {
            Racket cacheData = _cacheService.GetData<Racket>($"{id}");
            if (cacheData != null)
            {
                return cacheData;
            }

            Racket racket = _racketDbContext.Rackets.Find(id);
            if (racket is not null)
                _cacheService.SetData($"{racket.RacketId}", racket, DateTimeOffset.Now.AddDays(1));
            return racket;
        }

        public bool InsertRacket(Racket racket)
        {
            if (racket != null)
            {
                _racketDbContext.Rackets.Add(racket);
                if (_cacheService.GetData<Racket>($"{racket.RacketId}") is null)
                {
                    _cacheService.SetData($"{racket.RacketId}", racket, DateTimeOffset.Now.AddDays(1));
                }
            }
            return (_racketDbContext.SaveChanges() > 0);
        }

        public bool UpdateRacket(Racket racket)
        {
            _cacheService.RemoveData($"{racket.RacketId}");
            _cacheService.SetData($"{racket.RacketId}", racket, DateTimeOffset.Now.AddDays(1));
            bool modified = false;
            Racket? toModify = _racketDbContext.Rackets.SingleOrDefault(c => c.RacketId == racket.RacketId);
            if (toModify != null)
            {
                _racketDbContext.Rackets.Remove(toModify);
                _racketDbContext.Rackets.Add(racket);
                _racketDbContext.SaveChanges();
                modified = true;
            }

            return modified;
        }

        public IEnumerable<Racket> GetRacketByName(string name)
        {
            var result = (from racket in _racketDbContext.Rackets
                          where racket.Marca.Contains(name)
                          select racket).ToList();
            return result;
        }
    }
}
