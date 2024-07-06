using Grpc.Net.Client;
using Taxi.gRPC;

using var channel = GrpcChannel.ForAddress("https://localhost:6010");
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });

Console.WriteLine("Greeting: " + reply.Message);
Console.WriteLine("Press any key to exit...");

Console.ReadKey();