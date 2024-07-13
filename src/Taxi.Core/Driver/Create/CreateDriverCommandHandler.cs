using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Driver.Create
{
    public sealed class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, string>
    {
        private readonly IDriverRepository _driverRepository;

        public CreateDriverCommandHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<string> Handle(CreateDriverCommand command, CancellationToken cancellationToken)
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

            return await _driverRepository.Create(driver);
        }
    }
}
