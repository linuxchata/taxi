using System;
using MediatR;

namespace Taxi.Core.Driver.Delete
{
    public sealed class DeleteDriverCommand : IRequest
    {
        internal string Id { get; private set; }

        public DeleteDriverCommand(string id)
        {
            Id = id;
        }
    }
}
