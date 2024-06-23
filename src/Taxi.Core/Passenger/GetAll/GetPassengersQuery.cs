using MediatR;

namespace Taxi.Core.Passenger.GetAll
{
    public sealed class GetPassengersQuery : IRequest<GetPassengersResponse>
    {
    }
}
