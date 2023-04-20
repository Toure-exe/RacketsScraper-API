using Microsoft.AspNetCore.Identity;
using RacketsScrapper.Domain;
using RacketsScrapper.Domain.Identity;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IdentityServer.Services
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
            Console.WriteLine(">>>>>>> ENTRO");
            var result = await client.GetAsync("https://localhost:7142/connect/token");
            if (result.IsSuccessStatusCode) 
            {
                var model = await result.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<AccessToken>(model);
                return token.Access_Token;
            }
            else
            {
                return "";
            }

           
        }
    }
}
