using Grpc.Net.Client;
using Taxi.gRPC;

using var channel = GrpcChannel.ForAddress("https://localhost:6010");

var driverClient = new Driver.DriverClient(channel);
var driverReply = await driverClient.GetDriverAsync(new GetDriverRequest { Id = "6523780e-d6dc-424b-8e71-004afc8c7468" });
Console.WriteLine("Greeting driver {0} {1} from state of {2}", driverReply.FirstName, driverReply.LastName, driverReply.State);

var passengerClient = new Passenger.PassengerClient(channel);
var passengerReply = await passengerClient.GetPassengerAsync(new GetPassengerRequest { Id = "0e8ee2ba-f626-44d7-8681-63fd14a7f4e2" });
Console.WriteLine("Greeting passenger {0} {1} from state of {2}", passengerReply.FirstName, passengerReply.LastName, passengerReply.State);

Console.WriteLine("Press any key to exit...");

Console.ReadKey();