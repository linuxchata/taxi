using System;
using MediatR;

namespace Taxi.Core.Driver.Delete
{
    public sealed class DeleteDriverCommand : IRequest
    {
        internal Guid Id { get; private set; }

        internal string State { get; private set; }

        public DeleteDriverCommand(Guid id, string state)
        {
            Id = id;
            State = state;
        }
    }
}
