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
    }
}
