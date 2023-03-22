using RacketsScrapper.Domain;
using System;
using System.Collections.Generic;
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
        public Racket GetRacketById(int id)
        {
            throw new NotImplementedException();
        }

        public bool InsertRacket(Racket racket)
        {
            if (racket != null) 
            {
                _racketDbContext.Rackets.Add(racket);
            }
            return (_racketDbContext.SaveChanges() > 0);
        }
    }
}
