using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Google;
using RacketScraper.IdentityServer;
using RacketScraper.IdentityServer.Services;
using RacketsScrapper.Domain.Identity;
using RacketsScrapper.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddTestUsers(Config.TestUsers)
    .AddDeveloperSigningCredential();

builder.Services.AddDbContext<RacketDbContext>(opt
    => opt.UseSqlServer(builder.Configuration.GetConnectionString("DbString")));
builder.Services.AddScoped<IAuthService, AuthService>();


var identityBuilder = builder.Services.AddIdentityCore<User>(opt => opt.User.RequireUniqueEmail = true);
identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole), builder.Services);
identityBuilder.AddEntityFrameworkStores<RacketDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "600543089869-plavm2lirv6k30ti8mf76rgeicu28tr2.apps.googleusercontent.com";
    options.ClientSecret = "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";
});

builder.Services.AddIdentity<User, IdentityRole>();
builder.Services.AddAutoMapper(typeof(AutoMapping).Assembly);


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseIdentityServer();
//app.UseCors("corsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
