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
                State = command.Request.State,
            };

            await _driverRepository.Update(command.Id, driver);
        }
    }
}
