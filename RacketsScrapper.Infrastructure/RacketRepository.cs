using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            return _racketDbContext.Rackets.Take(10);
        }

        public bool DeleteRacket(Racket racket)
        {
            _cacheService.RemoveData($"{racket.RacketId}");
            _racketDbContext.Rackets.Remove(racket);
            return _racketDbContext.SaveChanges() > 0;
        }

        public ResponseFilterObject GetAllRackets(int currentPage)
        {
            if (_racketDbContext.Rackets is null || currentPage < 1)
                return null;

            var racketCtx = _racketDbContext.Rackets;
            float racketNumberByPage = 20f;
            double pagesNumber = Math.Ceiling(racketCtx.Count() / racketNumberByPage);
            var result = racketCtx
                .Skip((currentPage - 1) * (int)racketNumberByPage) // numero di elementi da ignorare
                .Take((int)racketNumberByPage) // elementi da prendere
                .ToList();

            ResponseFilterObject response = new ResponseObjectHome()
            {
                Rackets = result,
                Pages = pagesNumber,
                CurrentPage = currentPage,
                Sessi = (from rck in racketCtx
                         where rck.Sesso != null
                        select rck.Sesso).Distinct(),
                Marche = (from rck in racketCtx
                          where rck.Marca != null
                         select rck.Marca).Distinct(),
                Colori = ((from rck in racketCtx
                          where rck.ColoreUno != null
                         select rck.ColoreUno).Concat(from rck in racketCtx
                                                      where rck.ColoreDue != null
                                                      select rck.ColoreDue)).Distinct(),
                Elements = racketCtx.Count()
            };


            return response;
        }

        public ResponseFilterObject GetAllRacketsWithFilter(RequestFilterObject request, int page)
        {
            List<Racket> filteredList = new List<Racket>();
            if (_racketDbContext.Rackets is null)
                return null;

            var racketQuerybleList = _racketDbContext.Rackets.AsQueryable();
            float racketNumberByPage = 20f;

          /*  if(request.Keyword is not null)
            {
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    filteredList.AddRange((from racket in racketQuerybleList
                                           where racket.Marca.Contains(request.Keyword)
                                           || racket.Modello.Contains(request.Keyword)
                                           select racket));
                }
            }*/

            if (request.Colors is not null)
            {
                foreach (var color in request.Colors)
                {
                    filteredList.AddRange(racketQuerybleList.Where(racket => racket.ColoreUno.Contains(color)
                   || racket.ColoreDue.Contains(color) && !filteredList.Contains(racket)));
                }
            }
            if (request.SexList is not null)
            {
                foreach (var sex in request.SexList)
                {
                    filteredList.AddRange(racketQuerybleList.Where(racket => racket.Sesso == sex 
                                          && !filteredList.Contains(racket)));
                }

            }
            if (request.Brands is not null)
            {
                foreach (var brand in request.Brands)
                {
                    filteredList.AddRange(racketQuerybleList.Where(racket => racket.Marca.Contains(brand)
                                                        && !filteredList.Contains(racket)));
                }
            }
            if (request.Order == "asc")
            {
                if(!filteredList.IsNullOrEmpty())
                    filteredList = filteredList.OrderBy(item => item.Prezzo).ToList();
                else
                    filteredList = racketQuerybleList.OrderBy(item => item.Prezzo).ToList();
            }
            else if (request.Order == "desc")
            {
                if (!filteredList.IsNullOrEmpty())
                    filteredList = filteredList.OrderByDescending(item => item.Prezzo).ToList();
                else
                    filteredList = racketQuerybleList.OrderByDescending(item => item.Prezzo).ToList();
            }
            else
            {
                if (filteredList.IsNullOrEmpty())
                    filteredList = racketQuerybleList.ToList();
            }

            if (request.Keyword is not null)
            {
                filteredList = (from racket in filteredList
                                where racket.Marca.Contains(request.Keyword)
                               // || racket.Modello.Contains(request.Keyword)
                                select racket).ToList();
            }

            var responseList = filteredList.Skip((page - 1) * (int)racketNumberByPage)
             .Take((int)racketNumberByPage);
            double pagesNumber = Math.Ceiling(filteredList.Count / racketNumberByPage);

            ResponseFilterObject response = InstantiateResponseFilter(page, filteredList, responseList, pagesNumber);

            return response;
        }
     
        public Racket? GetRacketById(int id)
        {
            Racket cacheData = _cacheService.GetData<Racket>($"{id}");
            if (cacheData != null)
            {
                return cacheData;
            }

            Racket ?racket = _racketDbContext.Rackets.Find(id);
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

        public ResponseFilterObject? GetRacketByName(string name, int page)
        {
            IEnumerable<Racket>? result = null;
            ResponseFilterObject? response = null;
            double racketNumberByPage = 20f;
            if (!string.IsNullOrEmpty(name)) 
            {
                result = (from racket in _racketDbContext.Rackets
                          where racket.Marca.Contains(name) 
                          ||    racket.Modello.Contains(name)
                          select racket);

                IEnumerable<Racket>? filteredResult = result.Skip((page - 1) * (int)racketNumberByPage)
                    .Take((int) racketNumberByPage)
                    .ToList();
            
                double pagesNumber = Math.Ceiling(result.Count() / racketNumberByPage);
                response = new ResponseObjectHome()
                {
                    Rackets = filteredResult,
                    Pages = pagesNumber,
                    CurrentPage = page,
                    Sessi = (from rck in result
                             where !string.IsNullOrEmpty(rck.Sesso)
                                    && !string.IsNullOrWhiteSpace(rck.Sesso)
                             select rck.Sesso).Distinct(),
                    Marche = (from rck in result
                              where !string.IsNullOrEmpty(rck.Marca)
                                    && !string.IsNullOrWhiteSpace(rck.Marca)
                              select rck.Marca.ToLower()).Distinct(),
                    Colori = ((from rck in result
                              where !string.IsNullOrEmpty(rck.ColoreUno) &&
                                    !string.IsNullOrWhiteSpace(rck.ColoreUno)
                              select rck.ColoreUno).Concat(from rck in result
                                                           where !string.IsNullOrEmpty(rck.ColoreDue) &&
                                                                 !string.IsNullOrWhiteSpace(rck.ColoreDue)
                                                           select rck.ColoreDue)).Distinct(),
                    Elements = result.Count()
                };

            }

            return response;
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

        private ResponseFilterObject InstantiateResponseFilter(int page, List<Racket> filteredList, IEnumerable<Racket> responseList, double pagesNumber)
        {
            return new ResponseObjectHome()
            {
                Rackets = responseList,
                Pages = pagesNumber,
                CurrentPage = page,
                Elements = filteredList.Count,
                Sessi = (from rck in filteredList
                         where !string.IsNullOrEmpty(rck.Sesso)
                                && !string.IsNullOrWhiteSpace(rck.Sesso)
                         select rck.Sesso).Distinct(),
                Marche = (from rck in filteredList
                          where !string.IsNullOrEmpty(rck.Marca)
                                && !string.IsNullOrWhiteSpace(rck.Marca)
                          select rck.Marca.ToLower()).Distinct(),
                Colori = ((from rck in filteredList
                           where !string.IsNullOrEmpty(rck.ColoreUno) &&
                                 !string.IsNullOrWhiteSpace(rck.ColoreUno)
                           select rck.ColoreUno).Concat(from rck in filteredList
                                                        where !string.IsNullOrEmpty(rck.ColoreDue) &&
                                                              !string.IsNullOrWhiteSpace(rck.ColoreDue)
                                                        select rck.ColoreDue)).Distinct(),
            };
        }

    }
}
