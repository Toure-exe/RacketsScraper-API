using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RacketScrapper.API.Services;
using RacketsScrapper.Domain;
using RacketsScrapper.Domain.Identity;

namespace RacketScrapper.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public AuthenticationController(UserManager<User> userManager, IMapper mapper, IAuthService authService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _authService = authService;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> UserRegistration([FromBody] UserDTO userDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    User user = _mapper.Map<User>(userDto);
                    user.Email = userDto.EmailAddress;
                    if(!await _roleManager.RoleExistsAsync(userDto.Role))
                    {
                        return BadRequest("Il ruolo non esiste");
                    }
                    var status = await _userManager.CreateAsync(user, userDto.Password);
                    if(!status.Succeeded)
                    {
                        foreach(var error in  status.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                        return BadRequest(ModelState);
                    }

                    await _userManager.AddToRoleAsync(user, userDto.Role);

                    return Ok();

                }catch(Exception ex)
                {
                    return BadRequest("Qualcosa e' andato storto.");
                }
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                if (await _authService.ValidateUser(loginDto))
                {
                    return Accepted(new { Token = _authService.CreateToken() });
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Errore: qualcosa e' andato storto.");
            }
            
        }
    }
}
