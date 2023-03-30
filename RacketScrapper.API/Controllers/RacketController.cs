using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RacketsScrapper.Application;
using RacketsScrapper.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RacketScrapper.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RacketController : ControllerBase
    {

        private readonly IServiceDispatcher _serviceDispatcher;
        private readonly IRacketCrudService _racketCrudService;

        public RacketController(IServiceDispatcher serviceDispatcher, IRacketCrudService racketCrudService)
        {
            _serviceDispatcher = serviceDispatcher;
            _racketCrudService = racketCrudService;
        }


        [HttpPost("tennis-point/scrap")]
        public void GetDataFromTennisPoint()
        {
            // _racketCrudService.DeleteAllRackets();
            _serviceDispatcher.RunTennisPointScraper();
        }

        [HttpPost("padel-nuestro/scrap")]
        public void GetDataFromPadelNuestro()
        {
            // _racketCrudService.DeleteAllRackets();
            _serviceDispatcher.RunPadelNuestroScraper();
        }

        // DELETE api/<RacketController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteRacketById(int id)
        {
            return _racketCrudService.DeleteRacketbyId(id) ? Ok() : StatusCode(500);
        }

        // DELETE api/<RacketController>/5
        [HttpDelete]
        public IActionResult DeleteAllRacket()
        {
            return _racketCrudService.DeleteAllRackets() ? Ok() : StatusCode(500);
        }


        // GET: api/<RacketController>
        [EnableCors("corsPolicy")]
        [HttpGet("rackets/{currentPage}")]
        public IActionResult GetAllRackets(int currentPage)
        {
            //IEnumerable<Racket> result = _racketCrudService.GetTenRackets();
            ResponseObject result = _racketCrudService.GetAllRackets(currentPage);
            return (result != null) ? Ok(result) : NotFound();
        }

        // GET api/<RacketController>/5
        [HttpGet("{id}")]
        public IActionResult GetRacketById(int id)
        {
            Racket result = _racketCrudService.GetRacketById(id);
            return (result != null) ? Ok(result) : NotFound();
            
        }

        [HttpGet("search/{name}")]
        public IActionResult GetRacketByName(string name)
        {
            Console.WriteLine("ENTROOOOOOOOOO");
            IEnumerable<Racket> result = _racketCrudService.GetRacketByName(name);
            return (result != null) ? Ok(result) : NotFound();

        }

        // PUT api/<RacketController>/5
        [HttpPut("{id}")]
        public IActionResult ModifyRacket(int id, [FromBody] Racket value)
        {
            value.RacketId = id;
            return _racketCrudService.ModifyRacket(value) ? Ok() : StatusCode(500);
        }

        [HttpPost("sort/price-asc")]
        public IActionResult OrderByPriceAsc([FromBody] IEnumerable<Racket> values)
        {
            IEnumerable<Racket> result = _racketCrudService.OrderByPriceAsc(values);
            return (result != null) ? Ok(result) : BadRequest();

        }

        [HttpPost("sort/price-desc")]
        public IActionResult OrderByPriceDesc([FromBody] IEnumerable<Racket> values)
        {
            IEnumerable<Racket> result = _racketCrudService.OrderByPriceDesc(values);
            return (result != null) ? Ok(result) : BadRequest();

        }

    }
}
