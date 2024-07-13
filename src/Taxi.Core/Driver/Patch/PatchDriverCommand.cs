using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Taxi.Core.Driver.Patch
{
    public sealed class PatchDriverCommand : IRequest
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
