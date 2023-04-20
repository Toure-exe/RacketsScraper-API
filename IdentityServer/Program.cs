using IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RacketsScrapper.Domain.Identity;
using RacketsScrapper.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddTestUsers(Config.TestUsers)
    .AddDeveloperSigningCredential();

builder.Services.AddDbContext<RacketDbContext>(opt
    => opt.UseSqlServer(builder.Configuration.GetConnectionString("value")));

builder.Services.AddAuthentication();
var identityBuilder = builder.Services.AddIdentityCore<User>(opt => opt.User.RequireUniqueEmail = true);
identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole), builder.Services);
identityBuilder.AddEntityFrameworkStores<RacketDbContext>().AddDefaultTokenProviders();

builder.Services.AddIdentity<User, IdentityRole>();
builder.Services.AddAutoMapper(typeof(AutoMapping).Assembly);

var app = builder.Build();

app.UseIdentityServer();
app.MapGet("/", () => "Identity Server is on.");

app.Run();
