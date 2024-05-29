using System.Collections.Generic;
using MediatR;
using Taxi.Core.Driver.GetAll;

namespace Taxi.Core.Driver.GetDrivers
{
    public sealed class GetDriversQuery : IRequest<GetDriversResponse>
    {
    }
}
