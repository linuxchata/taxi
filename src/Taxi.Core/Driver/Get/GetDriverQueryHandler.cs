using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Driver.Get
{
    public sealed class GetDriverQueryHandler : IRequestHandler<GetDriverQuery, GetDriverBaseResponse>
    {
        private readonly IDriverRepository _driverRepository;

        public GetDriverQueryHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<GetDriverBaseResponse> Handle(GetDriverQuery query, CancellationToken cancellationToken)
        {
            var driver = await _driverRepository.GetById(query.Id);

            if (driver is null)
            {
                return new GetDriverNotFoundResponse();
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
