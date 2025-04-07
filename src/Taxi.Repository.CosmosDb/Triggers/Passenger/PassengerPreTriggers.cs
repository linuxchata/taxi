namespace Taxi.Repository.CosmosDb.Triggers.Passenger
{
    internal static class PassengerPreTriggers
    {
        internal const string ValidatePassengerPreTrigger = @"
            function validatePassengerPreTrigger() {
                var context = getContext();
                var request = context.getRequest();

                var itemToCreate = request.getBody();

                var isValid = itemToCreate.firstName && itemToCreate.lastName

                if (!isValid) {
                    throw new Error('Unable to create passenger due to failed validation');
                }
            }";
    }
}
