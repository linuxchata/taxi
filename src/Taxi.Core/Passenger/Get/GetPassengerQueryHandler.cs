using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Passenger.Get
{
    public sealed class GetPassengerQueryHandler : IRequestHandler<GetPassengerQuery, GetPassengerResponse>
    {
        private readonly IPassengerRepository _passengerRepository;

        public GetPassengerQueryHandler(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<GetPassengerResponse> Handle(GetPassengerQuery query, CancellationToken cancellationToken)
        {
            var driver = await _passengerRepository.GetById(query.Id);

            var mapped = new GetPassengerResponse
            {
                Id = driver.Id,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                State = driver.State,
            };

            return mapped;
        }
    }
}
