using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chicks.Api.Geo.Application.Events.AddGeocoderRequest
{
    public class PublishToRabbitMQHandler
        : INotificationHandler<AddGeocoderRequestEvent>
    {
        public IConnection Connection { get; }
        
        public PublishToRabbitMQHandler(RabbitMQ.Client.IConnection connection)
        {
            Connection = connection;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Handle(AddGeocoderRequestEvent notification, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var queue = "GeocoderRequest";

            var channel = Connection.CreateModel();

            var properties = channel.CreateBasicProperties();
            properties.Persistent = false;

            channel.QueueDeclare(queue, false, false, false, null);

            var command = JsonConvert.SerializeObject(new { 
                Request = notification.Command,
                Id = notification.Result.Id
            });

            byte[] messagebuffer = Encoding.UTF8.GetBytes(command);

            channel.BasicPublish(
                string.Empty,
                queue,
                properties,
                messagebuffer
            );
        }
    }
}
