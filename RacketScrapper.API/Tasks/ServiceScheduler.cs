using RacketsScrapper.Application;

namespace RacketScrapper.API.Tasks
{
    public class ServiceScheduler : BackgroundService
    {
        private readonly ILogger<ServiceScheduler> _logger;
        private readonly IServiceProvider _serviceProvider;
        public ServiceScheduler(ILogger<ServiceScheduler> logger, 
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var serviceDispatcher = scope.ServiceProvider.GetService<IServiceDispatcher>();
                    var racketCrudService = scope.ServiceProvider.GetService<IRacketCrudService>();
                    List<Task> tasks = new List<Task>();
                   /* tasks.Add(Task.Run(() => serviceDispatcher.RunTennisPointScraper()));
                    tasks.Add(Task.Run(() => serviceDispatcher.RunPadelNuestroScraper()));
                    await Task.WhenAll(tasks);*/
                    
                   /* racketCrudService.DeleteAllRackets();*/
                    _logger.LogInformation($"E' stato aggiornato il database eseguendo gli scraper il: {DateTime.Now} ");
                    await Task.Delay(TimeSpan.FromDays(1), stoppingToken);

                } 
            }
        }
    }
}
