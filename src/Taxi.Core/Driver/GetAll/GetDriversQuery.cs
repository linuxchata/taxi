using MediatR;

namespace Taxi.Core.Driver.GetAll
{
    public sealed class GetDriversQuery : IRequest<GetDriversResponse>
    {
    }
}
