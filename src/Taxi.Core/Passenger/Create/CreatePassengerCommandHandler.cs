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
                State = command.Request.State,
            };

            return await _passengerRepository.Create(driver);
        }
    }
}
