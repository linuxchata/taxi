using Grpc.Core;
using MediatR;
using Taxi.Core.Base;
using Taxi.Core.Driver.Get;

namespace Taxi.gRPC.Services;

public sealed class DriverService(IMediator mediator) : Driver.DriverBase
{
    public override async Task<GetDriverReply> GetDriver(GetDriverRequest request, ServerCallContext context)
    {
        var response = await mediator.Send(new GetDriverQuery(request.Id));

        var castedResponse = response as GetDriverResponse;

        if (castedResponse == null || response is NotFoundResponse)
        {
            return null!;
        }

        return new GetDriverReply
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
            IsApproved = castedResponse.IsApproved,
        };
    }
}