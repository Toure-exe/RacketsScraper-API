using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using RacketsScrapper.Domain.Identity;
using RacketsScrapper.Domain;

namespace RacketScraper.IdentityServer.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private User _user { get; set; }

        public AuthService(UserManager<User> userManager)
        {
            _userManager = userManager;
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
    }
}
