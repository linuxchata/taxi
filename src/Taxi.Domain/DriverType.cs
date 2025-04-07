using GraphQL.Types;

namespace Taxi.Domain;

public class DriverType : ObjectGraphType<Driver>
{
    public DriverType()
    {
        Field(_ => _.Id);
        Field(_ => _.FirstName);
        Field(_ => _.LastName);
        Field(_ => _.Email);
        Field(_ => _.PhoneNumber);
        Field(_ => _.Country);
        Field(_ => _.State);
    }
}