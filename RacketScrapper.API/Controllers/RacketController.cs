using Microsoft.AspNetCore.Authorization;
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
            _serviceDispatcher.RunTennisPointScraper();
        }

        [HttpPost("padel-nuestro/scrap")]
        public void GetDataFromPadelNuestro()
        {
            _serviceDispatcher.RunPadelNuestroScraper();
        }

        // DELETE api/<RacketController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteRacketById(int id)
        {
            
            return _racketCrudService.DeleteRacketbyId(id) ? Ok() : StatusCode(500);
        }

        // DELETE api/<RacketController>/5
        [Authorize]
        [HttpDelete]
        public IActionResult DeleteAllRacket()
        {
            return _racketCrudService.DeleteAllRackets() ? Ok() : StatusCode(500);
        }


        // GET: api/<RacketController>
        [Authorize]
        [EnableCors("corsPolicy")]
        [HttpGet("rackets/{currentPage}")]
        public IActionResult GetAllRackets(int currentPage)
        {
            ResponseFilterObject result = _racketCrudService.GetAllRackets(currentPage);
            return (result != null) ? Ok(result) : NotFound();
        }

        [Authorize]
        [EnableCors("corsPolicy")]
        [HttpPost("rackets/filter/{page}")]
        public IActionResult GetFilteredRacket([FromBody] RequestFilterObject request, int page)
        {
            ResponseFilterObject result = _racketCrudService.GetAllRacketsWithFilter(request,page);
            return (result != null) ? Ok(result) : NotFound();
        }


        // GET api/<RacketController>/5
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetRacketById(int id)
        {
            Console.WriteLine("->> " + this.User.Identity.Name);
            Racket result = _racketCrudService.GetRacketById(id);
            return (result != null) ? Ok(result) : NotFound();
            
        }

        [Authorize]
        [HttpGet("search/{name}")]
        public IActionResult GetRacketByName([FromQuery] int page,string name)
        {
            ResponseFilterObject result = _racketCrudService.GetRacketByName(name,page);
            return (result != null) ? Ok(result) : NotFound();

        }

        // PUT api/<RacketController>/5
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult ModifyRacket(int id, [FromBody] Racket value)
        {
            value.RacketId = id;
            return _racketCrudService.ModifyRacket(value) ? Ok() : StatusCode(500);
        }

        [Authorize]
        [HttpPost("rackets/insert")]
        public IActionResult insertRacket([FromBody] Racket value)
        {
            bool result = _racketCrudService.InsertRacket(value);
            return (result) ? Ok(result) : BadRequest();

        }

    }
}
