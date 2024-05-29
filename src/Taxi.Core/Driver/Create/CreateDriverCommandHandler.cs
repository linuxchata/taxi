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
                State = command.Request.State,
            };

            return await _driverRepository.Create(driver);
        }
    }
}
