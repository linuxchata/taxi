using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Taxi.Core.Base;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Driver.Patch
{
    public sealed class PatchDriverCommandHandler : IRequestHandler<PatchDriverCommand, BaseResponse>
    {
        private readonly IDriverRepository _driverRepository;

        public PatchDriverCommandHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<BaseResponse> Handle(PatchDriverCommand command, CancellationToken cancellationToken)
        {
            var driver = await _driverRepository.GetById(command.Id);

            if (driver is null)
            {
                return new NotFoundResponse();
            }

            PatchDriver(command, driver);

            var updatedDriver = await _driverRepository.Update(command.Id, driver);

            if (updatedDriver is null)
            {
                return new NotFoundResponse();
            }

            return new PatchDriverResponse();
        }

        private void PatchDriver(PatchDriverCommand command, Domain.Driver driver)
        {
            var operations = command.Request.Operations
                .Select(a => new Operation<Domain.Driver>
                {
                    op = a.op,
                    path = a.path,
                    from = a.from,
                    value = a.value,
                })
                .ToList();

            var patchDriver = new JsonPatchDocument<Domain.Driver>(
                operations,
                command.Request.ContractResolver);

            patchDriver.ApplyTo(driver);
        }
    }
}
