using Microsoft.IdentityModel.Tokens;
using RacketsScrapper.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace RacketScraper.IdentityServer.Services
{
    public interface IAuthService
    {
        public Task<bool> ValidateUser(LoginDTO user);

        public Task<string> CreateToken();

        public string GetToken(Payload user);

        public JwtSecurityToken GenerateToken(SigningCredentials credential, List<Claim> claims);

        public SigningCredentials GetSigninCredential();

        public List<Claim> GetClaims(Payload user);
    }
}
