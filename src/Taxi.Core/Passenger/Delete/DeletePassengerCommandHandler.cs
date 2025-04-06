using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taxi.Core.Base;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Passenger.Delete
{
    public sealed class DeletePassengerCommandHandler : IRequestHandler<DeletePassengerCommand, BaseResponse>
    {
        private readonly IPassengerRepository _passengerRepository;

        public DeletePassengerCommandHandler(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<BaseResponse> Handle(DeletePassengerCommand command, CancellationToken cancellationToken)
        {
            var isDriverDeleted = await _passengerRepository.Delete(command.Id);

            if (!isDriverDeleted)
            {
                return new NotFoundResponse();
            }

            return new DeletePassengerResponse();
        }
    }
}
