using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Taxi.Core.Driver.Create;
using Taxi.Core.Driver.Delete;
using Taxi.Core.Driver.Get;
using Taxi.Core.Driver.GetAll;
using Taxi.Core.Driver.Patch;
using Taxi.Core.Driver.Update;

namespace Taxi.WebApi.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]")]
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
    [ProducesResponseType<GetDriverResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<GetDriverResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<GetDriverResponse>> Get([FromRoute] string id)
    {
        var response = await _mediator.Send(new GetDriverQuery(id));

        return response switch
        {
            GetDriverNotFoundResponse => NotFound(string.Empty),
            GetDriverResponse => Ok(response),
            _ => StatusCode(501),
        };
    }

    /// <summary>
    /// Creates a driver
    /// </summary>
    /// <param name="request">Request to create a driver</param>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<string>> Create([FromBody][Required] CreateDriverRequest request)
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
        [FromBody][Required] UpdateDriverRequest request)
    {
        await _mediator.Send(new UpdateDriverCommand(id, request));
        return NoContent();
    }

    /// <summary>
    /// Patches the driver
    /// </summary>
    /// <param name="id">The identifier of the driver</param>
    /// <param name="request">Request to patch the driver</param>
    [HttpPatch("{id}")]
    [Consumes(MediaTypeNames.Application.JsonPatch)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Patch(
        [FromRoute] string id,
        [FromBody] JsonPatchDocument<PatchDriverRequest> request)
    {
        await _mediator.Send(new PatchDriverCommand(id, request));
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
