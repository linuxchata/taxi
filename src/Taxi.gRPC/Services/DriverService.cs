using Grpc.Core;
using MediatR;
using Taxi.Core.Driver.Get;

namespace Taxi.gRPC.Services;

public sealed class DriverService(IMediator mediator) : Driver.DriverBase
{
    public override async Task<GetDriverReply> GetDriver(GetDriverRequest request, ServerCallContext context)
    {
        var response = await mediator.Send(new GetDriverQuery(request.Id));
        return new GetDriverReply
        {
            Id = response.Id,
            FirstName = response.FirstName,
            LastName = response.LastName,
            State = response.State,
        };
    }
}