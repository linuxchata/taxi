using Grpc.Net.Client;
using Taxi.gRPC;

using var channel = GrpcChannel.ForAddress("https://localhost:6010");
var client = new Driver.DriverClient(channel);
var reply = await client.GetDriverAsync(new GetDriverRequest { Id = "6523780e-d6dc-424b-8e71-004afc8c7468" });

Console.WriteLine("Greeting: {0} {1} from state of {2}", reply.FirstName, reply.LastName, reply.State);
Console.WriteLine("Press any key to exit...");

Console.ReadKey();