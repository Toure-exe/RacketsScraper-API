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

        public RacketRepository(RacketDbContext racketDbContext)
        {
            _racketDbContext = racketDbContext;
        }

        public bool DeleteAllRackets()
        {
            foreach(Racket racket in _racketDbContext.Rackets)
            {
                _racketDbContext.Rackets.Remove(racket);
            }
            return _racketDbContext.SaveChanges() > 0;
        }

        public bool DeleteRacket(Racket racket)
        {
            _racketDbContext.Rackets.Remove(racket);
            return _racketDbContext.SaveChanges() > 0;
        }

        public Racket GetRacketById(int id) => _racketDbContext.Rackets.Find(id);

        public bool InsertRacket(Racket racket)
        {
            if (racket != null) 
            {
                _racketDbContext.Rackets.Add(racket);
            }
            return (_racketDbContext.SaveChanges() > 0);
        }

        public bool UpdateRacket(Racket racket)
        {
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
    }
}
