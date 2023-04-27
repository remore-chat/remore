using Remore.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Remore.Shared.Packets
{
    public class NotifyClientConnected : IPacket
    {
        public string PacketId => nameof(NotifyClientConnected);
        public RemorePublicClient ClientConnected { get; set; }

        public NotifyClientConnected(RemorePublicClient clientConnected)
        {
            ClientConnected = clientConnected;
        }

    }
}
