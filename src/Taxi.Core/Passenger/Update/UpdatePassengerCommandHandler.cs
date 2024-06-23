using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Passenger.Update
{
    public sealed class UpdatePassengerCommandHandler : IRequestHandler<UpdatePassengerCommand>
    {
        private readonly IPassengerRepository _passengerRepository;

        public UpdatePassengerCommandHandler(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task Handle(UpdatePassengerCommand command, CancellationToken cancellationToken)
        {
            var passenger = new Domain.Passenger
            {
                FirstName = command.Request.FirstName,
                LastName = command.Request.LastName,
                State = command.Request.State,
            };

            await _passengerRepository.Update(command.Id, passenger);
        }
    }
}
