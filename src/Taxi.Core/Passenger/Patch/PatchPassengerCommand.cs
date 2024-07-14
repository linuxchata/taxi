using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Taxi.Core.Base;

namespace Taxi.Core.Passenger.Patch
{
    public sealed class PatchPassengerCommand : IRequest<BaseResponse>
    {
        internal string Id { get; private set; }

        internal readonly JsonPatchDocument<PatchPassengerRequest> Request;

        public PatchPassengerCommand(string id, JsonPatchDocument<PatchPassengerRequest> request)
        {
            Id = id;
            Request = request;
        }
    }
}
