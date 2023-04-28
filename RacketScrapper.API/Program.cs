using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RacketScrapper.API;
using RacketScrapper.API.Tasks;
using RacketsScrapper.Application;
using RacketsScrapper.Domain.Identity;
using RacketsScrapper.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

/* ENABLE CORS POLICIES */
builder.Services.AddCors(options =>
{
options.AddPolicy("corsPolicy",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<RacketDbContext>(opt 
    => opt.UseSqlServer(builder.Configuration.GetConnectionString("value")));

/**IDENTITY CONFIG* */
builder.Services.AddAuthentication();
var identityBuilder = builder.Services.AddIdentityCore<User>(opt => opt.User.RequireUniqueEmail = true);
identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole), builder.Services);
identityBuilder.AddEntityFrameworkStores<RacketDbContext>().AddDefaultTokenProviders();

/** SCRAPER BACKGROUND TASK CONFIG */
builder.Services.AddHostedService<ServiceScheduler>();

/**DEFAULT CONFIG **/
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/** ENABLE JWT **/
//builder.Services.AddIdentity<User, IdentityRole>();
var jwtSettings = builder.Configuration.GetSection("Jwt");
string? key = Environment.GetEnvironmentVariable("RACKET_KEY");
/*builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.GetSection("Issuer").Value,
        ValidAudience = jwtSettings.GetSection("Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});*/

builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.ApiName = "racketEngine";
        options.Authority = "https://localhost:7011";
    });



/*builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("JWT", policy =>
    {
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });

    options.AddPolicy("SSO_GOOGLE", policy =>
    {
        policy.AuthenticationSchemes.Add(GoogleDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
});*/


/** ENABLE SSO **/

/*builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}).AddGoogle(options =>
{
    options.ClientId = "600543089869-plavm2lirv6k30ti8mf76rgeicu28tr2.apps.googleusercontent.com";
    options.ClientSecret = "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";
});*/

/**DEPENDENCY INJECTIONS* */
builder.Services.AddAutoMapper(typeof(AutoMapping).Assembly);
builder.Services.AddScoped<IRacketsRepository, RacketRepository>();
builder.Services.AddScoped<IRacketScraperService, TennisPointScraperService>();
builder.Services.AddScoped<IRacketScraperService, PadelNuestroScraperService>();
builder.Services.AddScoped<IRacketCrudService, RacketCrudService>();
builder.Services.AddScoped<IServiceDispatcher,  ServiceDispatcher>();
builder.Services.AddScoped<IDownloaderService, DownloaderService>();
builder.Services.AddScoped<ICacheService, CacheService>();

/** OPEN API (SWAGGER) DOCUMENTATION FOR JWT BEARER **/
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer' [space] YOUR_ACTUAL_TOKEN. " +
        "Example: 'Bearer dfbvdkngfkgedgn",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        { 
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "0Auth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }

    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("corsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
