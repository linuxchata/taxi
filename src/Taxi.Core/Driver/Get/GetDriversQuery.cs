using MediatR;

namespace Taxi.Core.Driver.Get
{
    public sealed class GetDriverQuery : IRequest<GetDriverBaseResponse>
    {
        internal string Id { get; private set; }

        public GetDriverQuery(string id)
        {
            Id = id;
        }
    }
}
