namespace Taxi.Repository.Triggers.Driver
{
    internal static class DriverPreTriggers
    {
        internal const string ValidateDriverPreTrigger = @"
            function validateDriverPreTrigger() {
                var context = getContext();
                var request = context.getRequest();

                var itemToCreate = request.getBody();

                var isValid = itemToCreate.firstName && itemToCreate.lastName

                if (!isValid) {
                    throw new Error('Unable to create driver due to failed validation');
                }
            }";
    }
}
