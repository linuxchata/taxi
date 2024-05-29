using System;
using System.Diagnostics.CodeAnalysis;

namespace Taxi.Core.Driver.Update
{
    public sealed class UpdateDriverRequest
    {
        [NotNull]
        public string FirstName { get; set; } = null!;

        [NotNull]
        public string LastName { get; set; } = null!;

        [NotNull]
        public string State { get; set; } = null!;
    }
}
