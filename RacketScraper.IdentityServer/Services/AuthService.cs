using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using RacketsScrapper.Domain.Identity;
using RacketsScrapper.Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace RacketScraper.IdentityServer.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private User _user { get; set; }

        public AuthService(UserManager<User> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<bool> ValidateUser(LoginDTO user)
        {
            _user = await _userManager.FindByEmailAsync(user.EmailAddress);
            if (_user != null)
            {
                return await _userManager.CheckPasswordAsync(_user, user.Password);
            }
            else
            {
                return false;
            }
        }

        public async Task<string> CreateToken()
        {
            using var client = new HttpClient();
            var parameters = new Dictionary<string, string>();
            parameters["grant_type"] = "client_credentials";
            parameters["scope"] = "racketEngine.read";
            parameters["client_id"] = "cwm.client";
            parameters["client_secret"] = "secret";
            var result = await client.PostAsync("https://localhost:7011/connect/token", new FormUrlEncodedContent(parameters));
            if (result.IsSuccessStatusCode)
            {
                var model = await result.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<AccessToken>(model);
                return token.Access_Token;
            }
            else
            {
                return "error";
            }


        }

        public string GetToken(Payload user)
        {
            var credential = GetSigninCredential();
            var claims = GetClaims(user);
            var token = GenerateToken(credential, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public JwtSecurityToken GenerateToken(SigningCredentials credential, List<Claim> claims)
        {
            var jwtSettings = _config.GetSection("Jwt");
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

        public SigningCredentials GetSigninCredential()
        {
            string? key = Environment.GetEnvironmentVariable("RACKET_KEY");
            SymmetricSecurityKey secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public List<Claim> GetClaims(Payload user)
        {
           
            List<Claim> claims = new List<Claim>
            {
                new Claim("username", user.GivenName+" "+user.FamilyName)
            };
  
            claims.Add(new Claim("role", "user"));
            claims.Add(new Claim("email", user.Email));

            return claims;

        }
    }
}
