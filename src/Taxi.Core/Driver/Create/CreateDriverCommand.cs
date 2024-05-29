using MediatR;

namespace Taxi.Core.Driver.Create
{
    public sealed class CreateDriverCommand : IRequest<string>
    {
        internal readonly CreateDriverRequest Request;

        public CreateDriverCommand(CreateDriverRequest request)
        {
            Request = request;
        }
    }
}
