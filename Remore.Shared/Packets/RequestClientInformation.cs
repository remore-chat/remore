using System;
using System.Collections.Generic;
using System.Text;

namespace Remore.Shared.Packets
{
    public class RequestClientInformation : IPacket
    {
        public string PacketId => nameof(RequestClientInformation);

    }
}
