using Grpc.Core;
using MediatR;
using Taxi.Core.Base;
using Taxi.Core.Passenger.Get;

namespace Taxi.gRPC.Services;

public sealed class PassengerService(IMediator mediator) : Passenger.PassengerBase
{
    public override async Task<GetPassengerReply> GetPassenger(GetPassengerRequest request, ServerCallContext context)
    {
        var response = await mediator.Send(new GetPassengerQuery(request.Id));

        var castedResponse = response as GetPassengerResponse;

        if (castedResponse == null || response is NotFoundResponse)
        {
            return null!;
        }

        return new GetPassengerReply
        {
            Id = castedResponse.Id,
            FirstName = castedResponse.FirstName,
            LastName = castedResponse.LastName,
            Email = castedResponse.Email,
            PhoneNumber = castedResponse.PhoneNumber,
            Country = castedResponse.Country,
            State = castedResponse.State,
            Rating = (double)castedResponse.Rating,
            IsActive = castedResponse.IsActive,
        };
    }
}