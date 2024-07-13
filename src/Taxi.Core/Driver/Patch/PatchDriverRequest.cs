namespace Taxi.Core.Driver.Patch
{
    public class PatchDriverRequest
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string State { get; set; } = null!;

        public bool IsActive { get; set; }

        public bool IsApproved { get; set; }
    }
}
