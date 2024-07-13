using MediatR;
using Taxi.Core.Base;

namespace Taxi.Core.Passenger.Delete
{
    public sealed class DeletePassengerCommand : IRequest<BaseResponse>
    {
        internal string Id { get; private set; }

        public DeletePassengerCommand(string id)
        {
            Id = id;
        }
    }
}
