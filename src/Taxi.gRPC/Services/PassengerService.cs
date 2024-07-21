using Google.Rpc;
using Grpc.Core;
using MediatR;
using Taxi.Core.Base;
using Taxi.Core.Passenger.Get;

namespace Taxi.gRPC.Services;

public sealed class PassengerService(IMediator mediator) : Passenger.PassengerBase
{
    /// <summary>
    /// Gets a passenger
    /// </summary>
    /// <param name="request">Get passenger request</param>
    /// <param name="context">Server call context</param>
    /// <returns>Returns a passenger</returns>
    public override async Task<GetPassengerReply> GetPassenger(GetPassengerRequest request, ServerCallContext context)
    {
        var response = await mediator.Send(new GetPassengerQuery(request.Id));

        var castedResponse = response as GetPassengerResponse;

        if (castedResponse == null || response is NotFoundResponse)
        {
            var status = new Google.Rpc.Status
            {
                Code = (int)Code.NotFound,
                Message = $"Passenger with identifier [{request.Id}] was not found",
            };

            throw status.ToRpcException();
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