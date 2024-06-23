using MediatR;

namespace Taxi.Core.Passenger.Get
{
    public sealed class GetPassengerQuery : IRequest<GetPassengerResponse>
    {
        internal string Id { get; private set; }

        public GetPassengerQuery(string id)
        {
            Id = id;
        }
    }
}
