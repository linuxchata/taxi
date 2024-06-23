using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Passenger.GetAll
{
    public sealed class GetPassengersQueryHandler : IRequestHandler<GetPassengersQuery, GetPassengersResponse>
    {
        private readonly IPassengerRepository _passengerRepository;

        public GetPassengersQueryHandler(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<GetPassengersResponse> Handle(GetPassengersQuery query, CancellationToken cancellationToken)
        {
            var passengers = await _passengerRepository.GetAll();

            var mapped = passengers.Select(d => new PassengerItem
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                State = d.State,
            });

            return new GetPassengersResponse { Items = mapped };
        }
    }
}
