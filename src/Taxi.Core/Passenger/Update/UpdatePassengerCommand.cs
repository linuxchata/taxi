using MediatR;

namespace Taxi.Core.Passenger.Update
{
    public sealed class UpdatePassengerCommand : IRequest
    {
        internal string Id { get; private set; }

        internal readonly UpdatePassengerRequest Request;

        public UpdatePassengerCommand(string id, UpdatePassengerRequest request)
        {
            Id = id;
            Request = request;
        }
    }
}
