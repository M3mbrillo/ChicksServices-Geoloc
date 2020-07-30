using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Chicks.Api.Geo.Application.Commands;
using Chicks.Api.Geo.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chicks.Api.Geo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GeolocalizarController : ControllerBase
    {
        public IMediator Mediator { get; }

        public GeolocalizarController(IMediator mediator)
        {
            Mediator = mediator;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreatedGeocoderRequestDto>> PostGeolocalizarAsync([FromBody] CreateGeocoderRequestCommand command, CancellationToken cancellationToken) {

            var result = await this.Mediator.Send(command, cancellationToken);

            return this.Accepted(result);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegisteredRequestGeocoder>> GetGeolocalizarAsync([FromQuery][Required] int? id)
        {
            throw new NotImplementedException();
        }

    }
}
