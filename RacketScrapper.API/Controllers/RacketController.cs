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

        private readonly IServiceDispatcher _servideDispatcher;
        private readonly IRacketCrudService _racketCrudService;

        public RacketController(IServiceDispatcher serviceDispatcher, IRacketCrudService racketCrudService)
        {
            _servideDispatcher = serviceDispatcher;
            _racketCrudService = racketCrudService;
        }


        [HttpPost("tennis-point/scrap")]
        public void GetDataFromTennisPoint()
        {
            _servideDispatcher.RunTennisPointScraper();
        }

        // DELETE api/<RacketController>/5
        [HttpDelete("tennis-point/{id}")]
        public IActionResult DeleteRacketById(int id)
        {
            return _racketCrudService.DeleteRacketbyId(id) ? Ok() : StatusCode(500);
        }

        // DELETE api/<RacketController>/5
        [HttpDelete("tennis-point")]
        public IActionResult DeleteAllRacket()
        {
            return _racketCrudService.DeleteAllRackets() ? Ok() : StatusCode(500);
        }


        // GET: api/<RacketController>
        [HttpGet("tennis-point")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<RacketController>/5
        [HttpGet("tennis-point/{id}")]
        public IActionResult GetRacketById(int id)
        {
            Racket result = _racketCrudService.GetRacketById(id);
            return (result != null)  ? Ok(result) : NotFound();
        }

        // PUT api/<RacketController>/5
        [HttpPut("tennis-point/{id}")]
        public IActionResult ModifyRacket(int id, [FromBody] Racket value)
        {
            value.RacketId = id;
           return _racketCrudService.ModifyRacket(value) ? Ok() : StatusCode(500);
        }
 
    }
}
