using System;
using MediatR;

namespace Taxi.Core.Driver.Update
{
    public sealed class UpdateDriverCommand : IRequest
    {
        internal Guid Id { get; private set; }

        internal string State { get; private set; }

        internal readonly UpdateDriverRequest Request;

        public UpdateDriverCommand(Guid id, string state, UpdateDriverRequest request)
        {
            Id = id;
            State = state;
            Request = request;
        }
    }
}
