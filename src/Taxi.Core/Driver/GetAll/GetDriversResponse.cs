using System.Collections.Generic;

namespace Taxi.Core.Driver.GetAll
{
    public sealed class GetDriversResponse
    {
        public IEnumerable<GetDriverResponse> Items { get; set; } = null!;
    }
}
