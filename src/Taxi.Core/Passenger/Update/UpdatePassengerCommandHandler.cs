using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taxi.Core.Base;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Passenger.Update
{
    public sealed class UpdatePassengerCommandHandler : IRequestHandler<UpdatePassengerCommand, BaseResponse>
    {
        private readonly IPassengerRepository _passengerRepository;

        public UpdatePassengerCommandHandler(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<BaseResponse> Handle(UpdatePassengerCommand command, CancellationToken cancellationToken)
        {
            var passenger = new Domain.Passenger
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

            var updatedPassenger = await _passengerRepository.Update(command.Id, passenger);

            if (updatedPassenger is null)
            {
                return new NotFoundResponse();
            }

            return new UpdatePassengerResponse();
        }
    }
}
