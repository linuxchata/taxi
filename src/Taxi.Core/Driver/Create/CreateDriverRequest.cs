using System.Diagnostics.CodeAnalysis;

namespace Taxi.Core.Driver.Create
{
    public sealed class CreateDriverRequest
    {
        [NotNull]
        public string FirstName { get; set; } = null!;

        [NotNull]
        public string LastName { get; set; } = null!;

        [NotNull]
        public string State { get; set; } = null!;
    }
}
