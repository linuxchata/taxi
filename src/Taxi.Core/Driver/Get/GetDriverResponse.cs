namespace Taxi.Core.Driver.Get
{
    public sealed class GetDriverResponse : GetDriverBaseResponse
    {
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string State { get; set; } = null!;

        public decimal Rating { get; set; }

        public bool IsActive { get; set; }

        public bool IsApproved { get; set; }
    }
}
