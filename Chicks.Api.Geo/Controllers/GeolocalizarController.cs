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
using Chicks.Api.Geo.Application.Queries;
using Chicks.Database.NoSql.Models;
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
        public async Task<ActionResult<CreatedGeocoderRequestDto>> PostGeolocalizarAsync(
            [FromBody] CreateGeocoderRequestCommand command, CancellationToken cancellationToken) 
        {

            //Send the command
            var result = await this.Mediator.Send(command, cancellationToken);

            //Notify the created request of geocoder
            await this.Mediator.Publish(
                new Application.Events.AddGeocoderRequestEvent() { 
                    Command = command,
                    Result = result
                }
                , cancellationToken);

            return this.Accepted(result);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GeocoderRequestResult>> GetGeolocalizarAsync([FromQuery][Required] int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var r = await this.Mediator.Send(new GetGeocoderQuery() { Id = id.Value });

            return Ok(r);
        }

    }
}
