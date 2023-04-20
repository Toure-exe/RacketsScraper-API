using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddAuthentication();
var identityBuilder = builder.Services.AddIdentityCore<User>(opt => opt.User.RequireUniqueEmail = true);
identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole), builder.Services);
identityBuilder.AddEntityFrameworkStores<RacketDbContext>().AddDefaultTokenProviders();

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
