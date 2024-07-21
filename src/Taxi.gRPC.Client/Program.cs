using Grpc.Net.Client;
using Taxi.gRPC;

using var channel = GrpcChannel.ForAddress("https://localhost:6010");

var driverClient = new Driver.DriverClient(channel);
var driverReply = await driverClient.GetDriverAsync(new GetDriverRequest { Id = "467d173f-5e04-4418-a33f-1f06ff1fd6ae" });
Console.WriteLine("Greeting driver {0} {1} with rating {2}", driverReply.FirstName, driverReply.LastName, driverReply.Rating);

var passengerClient = new Passenger.PassengerClient(channel);
var passengerReply = await passengerClient.GetPassengerAsync(new GetPassengerRequest { Id = "2e21f94b-52d2-4eaa-b828-52ba2802c22c" });
Console.WriteLine("Greeting passenger {0} {1} from state of {2}", passengerReply.FirstName, passengerReply.LastName, passengerReply.State);

Console.WriteLine("Press any key to exit...");

Console.ReadKey();