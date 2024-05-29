using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Driver.Delete
{
    public sealed class DeleteDriverCommandHandler : IRequestHandler<DeleteDriverCommand>
    {
        private readonly IDriverRepository _driverRepository;

        public DeleteDriverCommandHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task Handle(DeleteDriverCommand command, CancellationToken cancellationToken)
        {
            await _driverRepository.Delete(command.Id, command.State);
        }
    }
}
