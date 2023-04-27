using Remore.Server;
using Remore.Server.Models;
using Remore.Shared;
using Remore.Shared.ClientPackets;
using Remore.Shared.Packets;
using WatsonTcp;

internal class RemoreServer
{
    private WatsonTcpServer _tcpServer;
    private List<RemoreServerClient> _connectedClients;
   
    public RemoreServer()
    {
        _connectedClients = new();
        _tcpServer = new WatsonTcpServer("127.0.0.1", 9000);
        _tcpServer.Events.ClientConnected += OnClientConnected;
        _tcpServer.Events.MessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnClientConnected(object? sender, ConnectionEventArgs e)
    {
        // Fire and forget
        _ = ProcessConnectedClient(e);
    }

    private async Task ProcessConnectedClient(ConnectionEventArgs e)
    {
        //TODO: Log everything
        try
        {
            var syncResponse = _tcpServer.SendAndWaitJson(5000, e.Client.Guid, new RequestClientInformation());
            var response = syncResponse.ReadSyncResponseAsType<AdditionalInformationResponse>();
            if (response == null)
            {
                _tcpServer.DisconnectClient(e.Client.Guid);
                return;
            }
            if (!(response as IPacket).CheckIfPacketValid())
            {
                _tcpServer.DisconnectClient(e.Client.Guid, MessageStatus.Failure);
            }
            //TODO: Check if client version matches server version; otherwise disconnect user (no backwards compatibility i'm too lazy)
            var serverClient = new RemoreServerClient(e.Client, new(response.Name, e.Client.Guid));
            _connectedClients.Add(serverClient);
            _tcpServer.Broadcast(new NotifyClientConnected(serverClient.RemoreClient));
        }
        catch (TimeoutException)
        {
            _tcpServer.DisconnectClient(e.Client.Guid, MessageStatus.AuthFailure);
        }
    }

    public async Task StartBlocking()
    {
        _tcpServer.Start();
        await Task.Delay(-1);
    }
}