using RacketsScrapper.Domain;

namespace RacketScraper.IdentityServer.Services
{
    public interface IAuthService
    {
        public Task<bool> ValidateUser(LoginDTO user);

        public Task<string> CreateToken();
    }
}
