using MediatR;

namespace Taxi.Core.Passenger.Delete
{
    public sealed class DeletePassengerCommand : IRequest
    {
        internal string Id { get; private set; }

        public DeletePassengerCommand(string id)
        {
            Id = id;
        }
    }
}
