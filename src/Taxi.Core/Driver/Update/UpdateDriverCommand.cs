using MediatR;
using Taxi.Core.Base;

namespace Taxi.Core.Driver.Update
{
    public sealed class UpdateDriverCommand : IRequest<BaseResponse>
    {
        internal string Id { get; private set; }

        internal readonly UpdateDriverRequest Request;

        public UpdateDriverCommand(string id, UpdateDriverRequest request)
        {
            Id = id;
            Request = request;
        }
    }
}
