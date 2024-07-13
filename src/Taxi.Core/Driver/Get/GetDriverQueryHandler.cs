using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taxi.Core.Base;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Driver.Get
{
    public sealed class GetDriverQueryHandler : IRequestHandler<GetDriverQuery, BaseResponse>
    {
        private readonly IDriverRepository _driverRepository;

        public GetDriverQueryHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<BaseResponse> Handle(GetDriverQuery query, CancellationToken cancellationToken)
        {
            var driver = await _driverRepository.GetById(query.Id);

            if (driver is null)
            {
                return new NotFoundResponse();
            }

            var mapped = new GetDriverResponse
            {
                Id = driver.Id,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                Email = driver.Email,
                PhoneNumber = driver.PhoneNumber,
                Country = driver.Country,
                State = driver.State,
                Rating = driver.Rating,
                IsActive = driver.IsActive,
                IsApproved = driver.IsApproved,
            };

            return mapped;
        }
    }
}
