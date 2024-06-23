using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taxi.Core.Passenger.Create;
using Taxi.Core.Passenger.Delete;
using Taxi.Core.Passenger.Get;
using Taxi.Core.Passenger.GetAll;
using Taxi.Core.Passenger.Update;

namespace Taxi.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
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
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<GetPassengerResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<GetPassengerResponse>> Get([FromRoute] string id)
    {
        var response = await _mediator.Send(new GetPassengerQuery(id));
        return Ok(response);
    }

    /// <summary>
    /// Creates a passenger
    /// </summary>
    /// <param name="request">Request to create a passenger</param>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<string>> Create([FromBody][NotNull] CreatePassengerRequest request)
    {
        var response = await _mediator.Send(new CreatePassengerCommand(request));
        return CreatedAtAction(nameof(Create), new { id = response });
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
        await _mediator.Send(new UpdatePassengerCommand(id, request));
        return NoContent();
    }

    /// <summary>
    /// Deletes the passenger
    /// </summary>
    /// <param name="id">The identifier of the passenger</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] string id)
    {
        await _mediator.Send(new DeletePassengerCommand(id));
        return NoContent();
    }
}
