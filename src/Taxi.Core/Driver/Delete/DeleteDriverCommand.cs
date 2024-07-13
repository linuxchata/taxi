using MediatR;
using Taxi.Core.Base;

namespace Taxi.Core.Driver.Delete
{
    public sealed class DeleteDriverCommand : IRequest<BaseResponse>
    {
        internal string Id { get; private set; }

        public DeleteDriverCommand(string id)
        {
            Id = id;
        }
    }
}
