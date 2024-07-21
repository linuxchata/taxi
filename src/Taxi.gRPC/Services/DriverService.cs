using Google.Protobuf.WellKnownTypes;
using Google.Rpc;
using Grpc.Core;
using MediatR;
using Taxi.Core.Base;
using Taxi.Core.Driver.Get;

namespace Taxi.gRPC.Services;

public sealed class DriverService(IMediator mediator) : Driver.DriverBase
{
    /// <summary>
    /// Gets a driver
    /// </summary>
    /// <param name="request">Get driver request</param>
    /// <param name="context">Server call context</param>
    /// <returns>Returns a driver</returns>
    public override async Task<GetDriverReply> GetDriver(GetDriverRequest request, ServerCallContext context)
    {
        var response = await mediator.Send(new GetDriverQuery(request.Id));

        var castedResponse = response as GetDriverResponse;

        if (castedResponse == null || response is NotFoundResponse)
        {
            var status = new Google.Rpc.Status
            {
                Code = (int)Code.NotFound,
                Message = $"Driver with identifier [{request.Id}] was not found",
            };

            throw status.ToRpcException();
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