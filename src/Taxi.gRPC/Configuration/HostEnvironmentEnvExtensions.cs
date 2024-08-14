namespace Taxi.gRPC.Configuration;

public static class HostEnvironmentEnvExtensions
{
    private const string Local = nameof(Local);

    public static bool IsLocal(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment(Local);
    }
}
