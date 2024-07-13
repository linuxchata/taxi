using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Driver.Update
{
    public sealed class UpdateDriverCommandHandler : IRequestHandler<UpdateDriverCommand>
    {
        private readonly IDriverRepository _driverRepository;

        public UpdateDriverCommandHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task Handle(UpdateDriverCommand command, CancellationToken cancellationToken)
        {
            var driver = new Domain.Driver
            {
                FirstName = command.Request.FirstName,
                LastName = command.Request.LastName,
                Email = command.Request.Email,
                PhoneNumber = command.Request.PhoneNumber,
                Country = command.Request.Country,
                State = command.Request.State,
                Rating = command.Request.Rating,
                IsActive = command.Request.IsActive,
                IsApproved = command.Request.IsApproved,
            };

            await _driverRepository.Update(command.Id, driver);
        }
    }
}
