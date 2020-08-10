using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Chicks.Geocoder
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //TODO make a IEventBus and deprecate Application.RabbitMQClient();
            //https://github.com/dotnet-architecture/eShopOnContainers/tree/dev/src/BuildingBlocks/EventBus

            var queue = new Application.RabbitMQClient();
            var api = new Application.GeocoderApi();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);

                var message = queue.ReceiveMessage();

                if (message == null)
                    continue;

                var result = await api.GetGeocoderAsync(message);

                queue.PushGeocode(result);
            }
        }
    }
}
