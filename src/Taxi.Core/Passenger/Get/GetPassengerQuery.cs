using MediatR;
using Taxi.Core.Base;

namespace Taxi.Core.Passenger.Get
{
    public sealed class GetPassengerQuery : IRequest<BaseResponse>
    {
        internal string Id { get; private set; }

        public GetPassengerQuery(string id)
        {
            Id = id;
        }
    }
}
