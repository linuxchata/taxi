using MediatR;

namespace Taxi.Core.Driver.Update
{
    public sealed class UpdateDriverCommand : IRequest
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
