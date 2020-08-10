using Chicks.Database.NoSql;
using Chicks.Database.Sql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chicks.Api.Geo.HostedServices
{
    public class PullGeocodeResult : BackgroundService
    {
        public PullGeocodeResult(IServiceProvider services)
        {
            Services = services;
        }

        public IServiceProvider Services { get; }
        readonly string queueResult = "GeocoderResult";

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = this.Services.CreateScope();

            var repoSql = scope.ServiceProvider.GetRequiredService<ChicksRepositoryProviderEF>();
            var repoMongo = scope.ServiceProvider.GetRequiredService<ChicksRepositoryProviderMongo>();

            var connection = scope.ServiceProvider.GetRequiredService<IConnection>();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queueResult, false, false, false, null);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10000, stoppingToken);

                // TODO move to Aggregate layer of a DDD 
                // and encapsule the save in a sql and mongodb

                var result = channel.BasicGet(queueResult, true);
                if (result == null)
                    continue;
                var jsonStrign = Encoding.UTF8.GetString(result.Body.ToArray());

                var item = JsonConvert.DeserializeObject<QueueItemGeocodeResult>(jsonStrign);

                var geocode = repoSql.RequestGeocoder.Get(item.Id);                

                if (geocode == null)
                    continue;

                geocode.Latitude = (decimal)item.Latitude;
                geocode.Longitude = (decimal)item.Longitude;
                var tSql = repoSql.RequestGeocoder.SaveAsync(geocode);

                var geocoderResult = repoMongo.GeocoderResult.Get(x => x.RequerestId == geocode.Id).FirstOrDefault() ?? new Database.NoSql.Models.GeocoderResult() {                     
                    RequerestId = geocode.Id
                };                
                geocoderResult.Latitud = (decimal)item.Latitude;
                geocoderResult.Longitud = (decimal)item.Longitude;
                geocoderResult.Estado = "TERMINADO";
                var tMongo = repoMongo.GeocoderResult.SaveAsync(geocoderResult);

                await tSql;
                await tMongo;
            }
        }
    }

    internal class QueueItemGeocodeResult
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
