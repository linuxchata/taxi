using Grpc.Core;
using MediatR;
using Taxi.Core.Passenger.Get;

namespace Taxi.gRPC.Services;

public sealed class PassengerService(IMediator mediator) : Passenger.PassengerBase
{
    public override async Task<GetPassengerReply> GetPassenger(GetPassengerRequest request, ServerCallContext context)
    {
        var response = await mediator.Send(new GetPassengerQuery(request.Id));
        return new GetPassengerReply
        {
            Id = response.Id,
            FirstName = response.FirstName,
            LastName = response.LastName,
            State = response.State,
        };
    }
}