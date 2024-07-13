using Grpc.Net.Client;
using Taxi.gRPC;

using var channel = GrpcChannel.ForAddress("https://localhost:6010");

var driverClient = new Driver.DriverClient(channel);
var driverReply = await driverClient.GetDriverAsync(new GetDriverRequest { Id = "674b28aa-82ee-49d8-b4fb-14753445305b" });
Console.WriteLine("Greeting driver {0} {1} with rating {2}", driverReply.FirstName, driverReply.LastName, driverReply.Rating);

var passengerClient = new Passenger.PassengerClient(channel);
var passengerReply = await passengerClient.GetPassengerAsync(new GetPassengerRequest { Id = "ab5bbc2a-b163-400f-8bde-955252eceed1" });
Console.WriteLine("Greeting passenger {0} {1} from state of {2}", passengerReply.FirstName, passengerReply.LastName, passengerReply.State);

Console.WriteLine("Press any key to exit...");

Console.ReadKey();