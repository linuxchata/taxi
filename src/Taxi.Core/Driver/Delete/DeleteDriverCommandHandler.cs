using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taxi.Core.Base;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Driver.Delete
{
    public sealed class DeleteDriverCommandHandler : IRequestHandler<DeleteDriverCommand, BaseResponse>
    {
        private readonly IDriverRepository _driverRepository;

        public DeleteDriverCommandHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<BaseResponse> Handle(DeleteDriverCommand command, CancellationToken cancellationToken)
        {
            var isDriverDeleted = await _driverRepository.Delete(command.Id);

            if (!isDriverDeleted)
            {
                return new NotFoundResponse();
            }

            return new DeleteDriverResponse();
        }
    }
}
