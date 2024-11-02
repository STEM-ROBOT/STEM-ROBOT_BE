using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using STEM_ROBOT.BLL.HubClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{
    public class RealTimeSvc : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Random _random = new Random();
        public int randomNumber { get; private set; }

        public RealTimeSvc(IServiceProvider serviceProvider, Random random)
        {
            _serviceProvider = serviceProvider;
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var random = _random.Next(1, 100);
                using(var scope = _serviceProvider.CreateScope())
                {
                    var hubClient = scope.ServiceProvider.GetRequiredService<IHubContext<TournamentClient>>();
                    await hubClient.Clients.All.SendAsync("number",randomNumber);
                }
                await Task.Delay(1000,stoppingToken);
            }
        }
    }
}
