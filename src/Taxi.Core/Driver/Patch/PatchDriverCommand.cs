using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Taxi.Core.Base;

namespace Taxi.Core.Driver.Patch
{
    public sealed class PatchDriverCommand : IRequest<BaseResponse>
    {
        internal string Id { get; private set; }

        internal readonly JsonPatchDocument<PatchDriverRequest> Request;

        public PatchDriverCommand(string id, JsonPatchDocument<PatchDriverRequest> request)
        {
            Id = id;
            Request = request;
        }
    }
}
