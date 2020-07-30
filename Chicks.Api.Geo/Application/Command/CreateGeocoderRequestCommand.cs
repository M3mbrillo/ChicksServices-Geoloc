using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chicks.Api.Geo.Application.Commands
{
    public class CreateGeocoderRequestCommand
        : IRequest<CreatedGeocoderRequestDto>
    {

    }
}
