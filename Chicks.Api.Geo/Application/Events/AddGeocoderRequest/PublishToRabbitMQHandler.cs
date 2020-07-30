using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chicks.Api.Geo.Application.Events.AddGeocoderRequest
{
    public class PublishToRabbitMQHandler
        : INotificationHandler<AddGeocoderRequestEvent>
    {
        public Task Handle(AddGeocoderRequestEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
