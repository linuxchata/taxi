using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Taxi.Core.Helpers
{
    public static class PatchOperationHelper
    {
        public static Operation<T> Map<T>(Operation operation) where T : class
        {
            return new Operation<T>
            {
                op = operation.op,
                path = operation.path,
                from = operation.from,
                value = operation.value,
            };
        }
    }
}
