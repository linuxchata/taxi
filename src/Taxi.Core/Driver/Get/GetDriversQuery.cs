using MediatR;

namespace Taxi.Core.Driver.Get
{
    public sealed class GetDriverQuery : IRequest<GetDriverResponse>
    {
        internal string Id { get; private set; }

        public GetDriverQuery(string id)
        {
            Id = id;
        }
    }
}
