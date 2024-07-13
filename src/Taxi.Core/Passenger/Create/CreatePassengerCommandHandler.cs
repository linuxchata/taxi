using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Passenger.Create
{
    public sealed class CreatePassengerCommandHandler : IRequestHandler<CreatePassengerCommand, string>
    {
        private readonly IPassengerRepository _passengerRepository;

        public CreatePassengerCommandHandler(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<string> Handle(CreatePassengerCommand command, CancellationToken cancellationToken)
        {
            var driver = new Domain.Passenger
            {
                FirstName = command.Request.FirstName,
                LastName = command.Request.LastName,
                Email = command.Request.Email,
                PhoneNumber = command.Request.PhoneNumber,
                Country = command.Request.Country,
                State = command.Request.State,
                Rating = command.Request.Rating,
                IsActive = command.Request.IsActive,
            };

            return await _passengerRepository.Create(driver);
        }
    }
}
