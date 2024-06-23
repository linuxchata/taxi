using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taxi.Core.Infrastructure;

namespace Taxi.Core.Driver.GetAll
{
    public sealed class GetDriversQueryHandler : IRequestHandler<GetDriversQuery, GetDriversResponse>
    {
        private readonly IDriverRepository _driverRepository;

        public GetDriversQueryHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<GetDriversResponse> Handle(GetDriversQuery query, CancellationToken cancellationToken)
        {
            var drivers = await _driverRepository.GetAll();

            var mapped = drivers.Select(d => new DriverItem
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                State = d.State,
            });

            return new GetDriversResponse { Items = mapped };
        }
    }
}
