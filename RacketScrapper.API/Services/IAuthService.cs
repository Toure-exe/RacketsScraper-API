using RacketsScrapper.Domain;

namespace RacketScrapper.API.Services
{
    public interface IAuthService
    {
        public Task<bool> ValidateUser(LoginDTO user);

        public Task<string> CreateToken();
    }
}
