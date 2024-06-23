using MediatR;

namespace Taxi.Core.Passenger.Create
{
    public sealed class CreatePassengerCommand : IRequest<string>
    {
        internal readonly CreatePassengerRequest Request;

        public CreatePassengerCommand(CreatePassengerRequest request)
        {
            Request = request;
        }
    }
}
