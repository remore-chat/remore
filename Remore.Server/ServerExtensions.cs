using Remore.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WatsonTcp;

namespace Remore.Server
{
    public static class ServerExtensions
    {

        public static void Broadcast(this WatsonTcpServer server, IPacket packet)
        {
            // Is it a good idea to do it in parallel? We'll see later
            Parallel.ForEach(server.ListClients(), x =>
            {
                server.SendAsJson(x.Guid, packet);
            });
        }

        public static T ReadSyncResponseAsType<T>(this SyncResponse syncResponse)
        {
            ArgumentNullException.ThrowIfNull(syncResponse, nameof(syncResponse));

            var content = Encoding.UTF8.GetString(syncResponse.Data);
            var @object = JsonSerializer.Deserialize<T>(content);
            return @object;
        }
        public static bool SendAsJson(this WatsonTcpServer server, Guid guid, IPacket packet, Dictionary<string, object> metadata = null)
        {
            return server.Send(guid, JsonSerializer.Serialize(packet), metadata);
        }
        public static SyncResponse SendAndWaitJson(this WatsonTcpServer server, int timeoutMs, Guid guid, IPacket packet, Dictionary<string, object> metadata = null)
        {
            return server.SendAndWait(timeoutMs, guid, JsonSerializer.Serialize(packet), metadata);
        }
    }
}
