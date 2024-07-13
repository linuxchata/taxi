using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taxi.Core.Base;
using Taxi.Core.Passenger.Create;
using Taxi.Core.Passenger.Delete;
using Taxi.Core.Passenger.Get;
using Taxi.Core.Passenger.GetAll;
using Taxi.Core.Passenger.Update;

namespace Taxi.WebApi.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]")]
public class PassengerController(IMediator _mediator) : ControllerBase
{
    /// <summary>
    /// Gets all passengers
    /// </summary>
    /// <returns>Returns all passengers</returns>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<GetPassengersResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<GetPassengersResponse>> GetAll()
    {
        var response = await _mediator.Send(new GetPassengersQuery());
        return Ok(response.Items);
    }

    /// <summary>
    /// Gets a passenger
    /// </summary>
    /// <param name="id">The identifier of the passenger</param>
    /// <returns>Returns a passenger</returns>
    [HttpGet("{id}", Name = "GetPassenger")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<GetPassengerResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<GetPassengerResponse>> Get2([FromRoute] string id)
    {
        var response = await _mediator.Send(new GetPassengerQuery(id));

        return response switch
        {
            NotFoundResponse => NotFound(),
            GetPassengerResponse => Ok(response),
            _ => StatusCode(StatusCodes.Status501NotImplemented),
        };
    }

    /// <summary>
    /// Creates a passenger
    /// </summary>
    /// <param name="request">Request to create a passenger</param>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<string>> Create([FromBody][Required] CreatePassengerRequest request)
    {
        var response = await _mediator.Send(new CreatePassengerCommand(request));

        var responseValue = new { id = response };
        return CreatedAtRoute("GetPassenger", responseValue, responseValue);
    }

    /// <summary>
    /// Updates the passenger
    /// </summary>
    /// <param name="id">The identifier of the passenger</param>
    /// <param name="request">Request to update the passenger</param>
    [HttpPut("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Update(
        [FromRoute] string id,
        [FromBody][NotNull] UpdatePassengerRequest request)
    {
        var response = await _mediator.Send(new UpdatePassengerCommand(id, request));

        return response switch
        {
            NotFoundResponse => NotFound(),
            UpdatePassengerResponse => NoContent(),
            _ => StatusCode(StatusCodes.Status501NotImplemented),
        };
    }

    /// <summary>
    /// Deletes the passenger
    /// </summary>
    /// <param name="id">The identifier of the passenger</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] string id)
    {
        var response = await _mediator.Send(new DeletePassengerCommand(id));

        return response switch
        {
            NotFoundResponse => NotFound(),
            DeletePassengerResponse => NoContent(),
            _ => StatusCode(StatusCodes.Status501NotImplemented),
        };
    }
}
