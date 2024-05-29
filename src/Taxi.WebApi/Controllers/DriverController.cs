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
    [HttpGet(Name = "Get")]
    public async Task<ActionResult<List<string>>> Get()
    {
        var response = await _mediator.Send(new GetDriversQuery());
        return Ok(response.Items);
    }

    [HttpPost]
    public async Task<ActionResult<List<string>>> Create([FromBody][NotNull] CreateDriverRequest request)
    {
        var response = await _mediator.Send(new CreateDriverCommand(request));
        return Ok(response);
    }

    [HttpPut("{id}/{state}")]
    public async Task<ActionResult<List<string>>> Update(
        [FromRoute] Guid id,
        [FromRoute] string state,
        [FromBody][NotNull] UpdateDriverRequest request)
    {
        await _mediator.Send(new UpdateDriverCommand(id, state, request));
        return NoContent();
    }

    [HttpDelete("{id}/{state}")]
    public async Task<ActionResult<List<string>>> Delete([FromRoute] Guid id, [FromRoute] string state)
    {
        await _mediator.Send(new DeleteDriverCommand(id, state));
        return NoContent();
    }
}
