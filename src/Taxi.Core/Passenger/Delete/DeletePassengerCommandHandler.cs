using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Passenger.Delete
{
    public sealed class DeletePassengerCommandHandler : IRequestHandler<DeletePassengerCommand>
    {
        private readonly IPassengerRepository _passengerRepository;

        public DeletePassengerCommandHandler(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task Handle(DeletePassengerCommand command, CancellationToken cancellationToken)
        {
            await _passengerRepository.Delete(command.Id);
        }
    }
}
