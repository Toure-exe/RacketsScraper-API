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

        public Racket GetRacketById(int id);

    }
}
