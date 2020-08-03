using Chicks.Geocoder.Application.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chicks.Geocoder.Application
{
    class RabbitMQClient : IDisposable
    {           
        readonly string queueRequest = "GeocoderRequest";
        readonly string queueResult = "GeocoderResult";


        IModel channel;
        IConnection connection;

        public RabbitMQClient()
        {            
            var connectionFactory = new RabbitMQ.Client.ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "rabbitmq"
            };

            this.connection = connectionFactory.CreateConnection();
            this.channel = connection.CreateModel();
            
            channel.QueueDeclare(queueRequest, false, false, false, null);
            channel.QueueDeclare(queueResult, false, false, false, null);
        }

        public void Dispose()
        {
            this.connection.Dispose();
            this.channel.Dispose();
        }

        public Models.QueueItemGeocodeRequest ReceiveMessage() {
            var consumer = new EventingBasicConsumer(this.channel);
            
            var result = channel.BasicGet(queueRequest, true);

            if (result == null)
                return null;

            var jsonStrign = Encoding.UTF8.GetString(result.Body.ToArray());

            return JsonConvert.DeserializeObject<Models.QueueItemGeocodeRequest>(jsonStrign);
        }

        public void PushGeocode(GeocodeResponse result)
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = false;

            var data = JsonConvert.SerializeObject(result);

            byte[] messagebuffer = Encoding.UTF8.GetBytes(data);

            channel.BasicPublish(
                string.Empty,
                queueResult,
                properties,
                messagebuffer
            );
        }
    }
}
