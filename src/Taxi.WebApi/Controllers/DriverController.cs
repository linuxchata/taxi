using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taxi.Core.Driver.Create;
using Taxi.Core.Driver.Delete;
using Taxi.Core.Driver.Get;
using Taxi.Core.Driver.GetAll;
using Taxi.Core.Driver.Update;

namespace Taxi.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DriverController(IMediator _mediator) : ControllerBase
{
    /// <summary>
    /// Gets all drivers
    /// </summary>
    /// <returns>Returns all drivers</returns>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<GetDriversResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<GetDriversResponse>> GetAll()
    {
        var response = await _mediator.Send(new GetDriversQuery());
        return Ok(response.Items);
    }

    /// <summary>
    /// Gets a driver
    /// </summary>
    /// <param name="id">The identifier of the driver</param>
    /// <returns>Returns a driver</returns>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<Core.Driver.Get.GetDriverResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<Core.Driver.Get.GetDriverResponse>> Get([FromRoute] string id)
    {
        var response = await _mediator.Send(new GetDriverQuery(id));
        return Ok(response);
    }

    /// <summary>
    /// Creates a driver
    /// </summary>
    /// <param name="request">Request to create a driver</param>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<string>> Create([FromBody][NotNull] CreateDriverRequest request)
    {
        var response = await _mediator.Send(new CreateDriverCommand(request));
        return CreatedAtAction(nameof(Create), new { id = response });
    }

    /// <summary>
    /// Updates the driver
    /// </summary>
    /// <param name="id">The identifier of the driver</param>
    /// <param name="request">Request to update the driver</param>
    [HttpPut("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Update(
        [FromRoute] string id,
        [FromBody][NotNull] UpdateDriverRequest request)
    {
        await _mediator.Send(new UpdateDriverCommand(id, request));
        return NoContent();
    }

    /// <summary>
    /// Deletes the driver
    /// </summary>
    /// <param name="id">The identifier of the driver</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] string id)
    {
        await _mediator.Send(new DeleteDriverCommand(id));
        return NoContent();
    }
}
