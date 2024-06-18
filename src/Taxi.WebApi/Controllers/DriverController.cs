using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taxi.Core.Driver.Create;
using Taxi.Core.Driver.Delete;
using Taxi.Core.Driver.GetDrivers;
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
    [HttpGet(Name = "Get")]
    public async Task<ActionResult<List<string>>> Get()
    {
        var response = await _mediator.Send(new GetDriversQuery());
        return Ok(response.Items);
    }

    /// <summary>
    /// Creates the driver
    /// </summary>
    /// <param name="request">Request to create a driver</param>
    [HttpPost]
    public async Task<ActionResult<List<string>>> Create([FromBody][NotNull] CreateDriverRequest request)
    {
        var response = await _mediator.Send(new CreateDriverCommand(request));
        return Ok(response);
    }

    /// <summary>
    /// Updates the driver
    /// </summary>
    /// <param name="id">The identifier of the driver</param>
    /// <param name="request">Request to update the driver</param>
    [HttpPut("{id}")]
    public async Task<ActionResult<List<string>>> Update(
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
    public async Task<ActionResult<List<string>>> Delete([FromRoute] string id)
    {
        await _mediator.Send(new DeleteDriverCommand(id));
        return NoContent();
    }
}
