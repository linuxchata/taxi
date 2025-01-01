using Grpc.Net.Client;
using Taxi.gRPC;

using var channel = GrpcChannel.ForAddress("https://localhost:6011");

var driverClient = new Driver.DriverClient(channel);
var driverReply = await driverClient.GetDriverAsync(new GetDriverRequest { Id = "49d7adaf-40c5-4619-974a-16228a75037c" });
Console.WriteLine("Greeting driver {0} {1} with rating {2}", driverReply.FirstName, driverReply.LastName, driverReply.Rating);

var passengerClient = new Passenger.PassengerClient(channel);
var passengerReply = await passengerClient.GetPassengerAsync(new GetPassengerRequest { Id = "c96335ac-02ad-4c4a-bfce-9aa03eb31ecd" });
Console.WriteLine("Greeting passenger {0} {1} from state of {2}", passengerReply.FirstName, passengerReply.LastName, passengerReply.State);

Console.WriteLine("Press any key to exit...");

Console.ReadKey();