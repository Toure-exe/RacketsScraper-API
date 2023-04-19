using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RacketsScrapper.Domain;
using RacketsScrapper.Domain.Identity;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RacketScrapper.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User _user { get; set; }

        public AuthService(UserManager<User> userManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<string> CreateToken()
        {
            var credential = GetSigninCredential();
            var claims = await GetClaims();
            var token = GenerateToken(credential, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateToken(SigningCredentials credential, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var expiration = DateTime.Now.AddMinutes(10);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                audience: jwtSettings.GetSection("Audience").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: credential
            );

            return token;
        }

        private async Task<List<Claim>> GetClaims()
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("username", _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach(var role in roles)
            {
                claims.Add(new Claim("role", role));
            }
            string email = await _userManager.GetEmailAsync(_user);
            claims.Add(new Claim("email", email));

            return claims;

        }

        private SigningCredentials GetSigninCredential()
        {
            string? key = Environment.GetEnvironmentVariable("RACKET_KEY");
            SymmetricSecurityKey secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<bool> ValidateUser(LoginDTO user)
        {
            _user = await _userManager.FindByEmailAsync(user.EmailAddress);
            if(_user != null)
            {
                return await _userManager.CheckPasswordAsync(_user,user.Password);
            }
            else
            {
                return false;
            }
        }
    }
}
