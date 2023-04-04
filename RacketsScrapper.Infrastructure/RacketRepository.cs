using Microsoft.EntityFrameworkCore;
using RacketsScrapper.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RacketsScrapper.Infrastructure
{
    public class RacketRepository : IRacketsRepository
    {
        public readonly RacketDbContext _racketDbContext;
        private readonly ICacheService _cacheService;
        private List<Racket> filteredList;

        public RacketRepository(RacketDbContext racketDbContext, ICacheService cacheService)
        {
            _racketDbContext = racketDbContext;
            _cacheService = cacheService;
            //filteredList = new();
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

        public ResponseObject GetAllRackets(int currentPage)
        {
            if (_racketDbContext.Rackets is null || currentPage < 1)
                return null;

            float racketNumberByPage = 20f;
            double pagesNumber = Math.Ceiling(_racketDbContext.Rackets.Count() / racketNumberByPage);
            var result = _racketDbContext.Rackets
                .Skip((currentPage - 1) * (int)racketNumberByPage) // numero di elementi da ignorare
                .Take((int)racketNumberByPage) // elementi da prendere
                .ToList();

            ResponseObject response = new ResponseObjectHome()
            {
                Rackets = result,
                Pages = pagesNumber,
                CurrentPage = currentPage,
                Sessi = (from rck in _racketDbContext.Rackets
                        select rck.Sesso).Distinct(),
                Marche = (from rck in _racketDbContext.Rackets
                         select rck.Marca).Distinct(),
                Colori = (from rck in _racketDbContext.Rackets
                         select rck.ColoreUno).Distinct(),
                Elements = _racketDbContext.Rackets.Count()
            };


            return response;
        }

        public ResponseObject GetAllRacketsWithFilter(RequestObject request, int page)
        {
            this.filteredList = new();
            if (_racketDbContext.Rackets is null)
                return null;

            float racketNumberByPage = 20f;
            
            if(request.Colors is not null)
            {
                foreach(var color in request.Colors)
                {
                    this.filteredList.AddRange(_racketDbContext.Rackets.Where(racket => racket.ColoreUno.Contains(color)
                   || racket.ColoreDue.Contains(color)));
                }
            }
            if (request.SexList is not null)
            {
                foreach(var sex in request.SexList)
                {
                    this.filteredList.AddRange( _racketDbContext.Rackets.Where(racket => racket.Sesso == sex));
                }

            }
            if(request.Brands is not null)
            {
                foreach(var brand in request.Brands)
                {
                    this.filteredList.AddRange(_racketDbContext.Rackets.Where(racket => racket.Marca.Contains(brand)));
                }
            }
            if(request.Order == "asc")
            {
                this.filteredList = this.filteredList.OrderBy(item => item.Prezzo).ToList();
            }
            else if(request.Order == "desc")
            {
                this.filteredList = this.filteredList.OrderByDescending(item => item.Prezzo).ToList();
            }
            

           var responseList = this.filteredList.Skip((page - 1) * (int)racketNumberByPage) 
            .Take((int)racketNumberByPage) 
            .ToList();
            double pagesNumber = Math.Ceiling(this.filteredList.Count() / racketNumberByPage);

            ResponseObject response = new ResponseObject()
            {
                Rackets = responseList,
                Pages = pagesNumber,
                CurrentPage = page,
                Elements = filteredList.Count()
            };


            return response;
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

        public IEnumerable<Racket> GetRacketByName(string name, int page)
        {
            IEnumerable<Racket>? result = null;
            double racketNumberByPage = 20f;
            if (!string.IsNullOrEmpty(name)) 
            {
                result = (from racket in _racketDbContext.Rackets
                          where racket.Marca.Contains(name) 
                          ||    racket.Modello.Contains(name)
                          select racket);

                return result.Skip((page - 1) * (int)racketNumberByPage)
                    .Take((int) racketNumberByPage)
                    .ToList();
            }

                 
            return result;
        }

        public IEnumerable<Racket> OrderByPriceAsc(IEnumerable<Racket> values)
        {
            return values.OrderBy(racket => racket.Prezzo);
        }

        public IEnumerable<Racket> OrderByPriceDesc(IEnumerable<Racket> values)
        {
            return (from racket in values
                   orderby racket.Prezzo descending
                   select racket);
        }

        public ResponseObject IndexPage(int page)
        {
            double racketNumberByPage = 20f;
            var responseList = this.filteredList.Skip((page - 1) * (int)racketNumberByPage)
            .Take((int)racketNumberByPage)
            .ToList();
            double pagesNumber = Math.Ceiling(this.filteredList.Count() / racketNumberByPage);

            ResponseObject response = new ResponseObject()
            {
                Rackets = responseList,
                Pages = pagesNumber,
                CurrentPage = page,
            };

            return response;
        }
    }
}
