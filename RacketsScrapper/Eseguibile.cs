using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RacketsScrapper.Application;
using RacketsScrapper.Infrastructure;

var serviceProvider = new ServiceCollection()
            .AddDbContext<RacketDbContext>(opt => opt.UseSqlServer("Data Source = AK24730\\MSSQLSERVER01; Initial Catalog= db_racchette; Integrated Security=true; TrustServerCertificate=True"))
            .AddScoped<ITennisPointScraperService, TennisPointScraperService>()
            .AddScoped<IDownloaderService, DownloaderService>()
            .AddScoped<IRacketsRepository, RacketRepository>()
            .BuildServiceProvider();

ITennisPointScraperService? _racketsScrapper = serviceProvider.GetService<ITennisPointScraperService>();
_racketsScrapper.GetPageHtmlCode("https://www.tennis-point.it/padel-racchette-da-padel/?start=0&sz=36");
string ?url = _racketsScrapper.getNextPageLink();
while (url != null)
{
    Console.WriteLine("PAGINA: "+_racketsScrapper.GetCurrentPage());
    _racketsScrapper.ReadAllRacketsLinks();
    _racketsScrapper.TakeRacketsData();
    _racketsScrapper.GetPageHtmlCode(url);
   url = _racketsScrapper.getNextPageLink();
    Console.WriteLine(">>>>NEXT PAGE LINK: " + url);
    _racketsScrapper.CleanLinkList();
}



//Console.WriteLine("\n\n NEXT PAGE LINK: "+_racketsScrapper.getNextPageLink());
