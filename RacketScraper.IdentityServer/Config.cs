using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace RacketScraper.IdentityServer
{
    public class Config
    {

        public static List<TestUser> TestUsers =>
      new List<TestUser>
      {
            new TestUser
            {
                SubjectId = "1144",
                Username = "admin_",
                Password = "Admin123@",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Mario Rossi"),
                    new Claim(JwtClaimTypes.GivenName, "Mario"),
                    new Claim(JwtClaimTypes.FamilyName, "Rossi"),
                    new Claim(JwtClaimTypes.Email, "real-admin@exemple.com"),
                }
            }
       };

        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("racketEngine.read"),
            new ApiScope("racketEngine.write"),
        };

        public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("racketEngine")
            {
                Scopes = new List<string>{ "racketEngine.read", "racketEngine.write" },
                ApiSecrets = new List<Secret>{ new Secret("racketEngine".Sha256()) }
            }
        };


        //grant
        public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "cwm.client",
                ClientName = "Client Credentials Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "racketEngine.read" }
            },
        };
    }
}
