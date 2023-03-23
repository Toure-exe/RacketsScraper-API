using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RacketsScrapper.Application;
using RacketsScrapper.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RacketDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("value")));
builder.Services.AddScoped<IRacketsRepository, RacketRepository>();
builder.Services.AddScoped<ITennisPointScraperService, TennisPointScraperService>();
builder.Services.AddScoped<IRacketCrudService, RacketCrudService>();
builder.Services.AddScoped<IServiceDispatcher,  ServiceDispatcher>();
builder.Services.AddScoped<IDownloaderService, DownloaderService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
