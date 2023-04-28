using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RacketScraper.IdentityServer.Services;
using RacketsScrapper.Domain;
using RacketsScrapper.Domain.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RacketScraper.IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        public IdentityController(UserManager<User> userManager, IMapper mapper, IAuthService authService,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _authService = authService;
            _roleManager = roleManager;
            _config = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> UserRegistration([FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    string errorMsg = "";
                    User user = _mapper.Map<User>(userDto);
                    user.Email = userDto.EmailAddress;
                    if (!await _roleManager.RoleExistsAsync(userDto.Role))
                    {
                        return BadRequest("Il ruolo non esiste");
                    }
                    var status = await _userManager.CreateAsync(user, userDto.Password);
                    if (!status.Succeeded)
                    {
                        foreach (var error in status.Errors)
                        {
                            errorMsg = " " + error.Description + "\n";
                        }
                        return BadRequest(errorMsg);
                    }

                    await _userManager.AddToRoleAsync(user, userDto.Role);

                    return Ok();

                }
                catch (Exception ex)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Errore: qualcosa e' andato storto.");
            }

        }

        [HttpPost("auth-google")]
        public async Task<IActionResult> GoogleAuth([FromBody] string credentials)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _config.GetSection("GoogleInfo").GetSection("clientId").Value }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credentials, settings);

            string jwt = _authService.GetToken(payload);

            return Ok(jwt);
            
        }

    
    }
}
