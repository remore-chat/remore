using Remore.Shared.ClientPackets;
using System.Text.Json;

Console.WriteLine(JsonSerializer.Serialize(new AdditionalInformationResponse("123", "222")));
var server = new RemoreServer();
await server.StartBlocking();