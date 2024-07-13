using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taxi.Core.Base;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Passenger.Get
{
    public sealed class GetPassengerQueryHandler : IRequestHandler<GetPassengerQuery, BaseResponse>
    {
        private readonly IPassengerRepository _passengerRepository;

        public GetPassengerQueryHandler(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<BaseResponse> Handle(GetPassengerQuery query, CancellationToken cancellationToken)
        {
            var passenger = await _passengerRepository.GetById(query.Id);

            if (passenger is null)
            {
                return new NotFoundResponse();
            }

            var mapped = new GetPassengerResponse
            {
                Id = passenger.Id,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                Email = passenger.Email,
                PhoneNumber = passenger.PhoneNumber,
                Country = passenger.Country,
                State = passenger.State,
                Rating = passenger.Rating,
                IsActive = passenger.IsActive,
            };

            return mapped;
        }
    }
}
