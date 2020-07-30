using MediatR;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chicks.Api.Geo.Application.Commands
{
    public class CreateGeocoderRequestHandler
        : IRequestHandler<CreateGeocoderRequestCommand, CreatedGeocoderRequestDto>
    {
        public Task<CreatedGeocoderRequestDto> Handle(CreateGeocoderRequestCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class CreatedGeocoderRequestDto { 
    
    }
}
