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

        public IEnumerable<Racket> GetAllRackets();

        IEnumerable<Racket> GetRacketByName(string name);

    }
}
