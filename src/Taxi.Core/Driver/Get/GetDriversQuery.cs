using MediatR;
using Taxi.Core.Base;

namespace Taxi.Core.Driver.Get
{
    public sealed class GetDriverQuery : IRequest<BaseResponse>
    {
        internal string Id { get; private set; }

        public GetDriverQuery(string id)
        {
            Id = id;
        }
    }
}
