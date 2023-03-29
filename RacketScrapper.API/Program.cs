using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RacketsScrapper.Application;
using RacketsScrapper.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RacketDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("value")));
builder.Services.AddScoped<IRacketsRepository, RacketRepository>();
builder.Services.AddScoped<IRacketScraperService, TennisPointScraperService>();
builder.Services.AddScoped<IRacketScraperService, PadelNuestroScraperService>();
builder.Services.AddScoped<IRacketCrudService, RacketCrudService>();
builder.Services.AddScoped<IServiceDispatcher,  ServiceDispatcher>();
builder.Services.AddScoped<IDownloaderService, DownloaderService>();
builder.Services.AddScoped<ICacheService, CacheService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("corsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
