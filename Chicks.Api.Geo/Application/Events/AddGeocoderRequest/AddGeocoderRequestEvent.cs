using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chicks.Api.Geo.Application.Events
{
    public class AddGeocoderRequestEvent : INotification
    {
        public Commands.CreateGeocoderRequestCommand Command { get; set; }
        public Commands.CreatedGeocoderRequestDto Result { get; set; }
    }
}
