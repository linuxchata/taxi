using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Taxi.Core.Base;
using Taxi.Core.Helpers;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Passenger.Patch
{
    public sealed class PatchPassengerCommandHandler : IRequestHandler<PatchPassengerCommand, BaseResponse>
    {
        private readonly IPassengerRepository _passengerRepository;

        public PatchPassengerCommandHandler(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<BaseResponse> Handle(PatchPassengerCommand command, CancellationToken cancellationToken)
        {
            var passenger = await _passengerRepository.GetById(command.Id);

            if (passenger is null)
            {
                return new NotFoundResponse();
            }

            PatchPassenger(command, passenger);

            var updatedPassenger = await _passengerRepository.Update(command.Id, passenger);

            if (updatedPassenger is null)
            {
                return new NotFoundResponse();
            }

            return new PatchPassengerResponse();
        }

        private void PatchPassenger(PatchPassengerCommand command, Domain.Passenger passenger)
        {
            var operations = command.Request.Operations
                .Select(PatchOperationHelper.Map<Domain.Passenger>)
                .ToList();

            var patchedPassenger = new JsonPatchDocument<Domain.Passenger>(
                operations,
                command.Request.ContractResolver);

            patchedPassenger.ApplyTo(passenger);
        }
    }
}
