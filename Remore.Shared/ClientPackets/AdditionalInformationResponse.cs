using System;
using System.Collections.Generic;
using System.Text;

namespace Remore.Shared.ClientPackets
{
    public class AdditionalInformationResponse : IPacket
    {
        public string PacketId => nameof(AdditionalInformationResponse);
        public string Name { get; set; }
        public string ClientVersion { get; set; }
        public AdditionalInformationResponse()
        {
            
        }
        public AdditionalInformationResponse(string name, string clientVersion)
        {
            Name = name;
            ClientVersion = clientVersion;
        }
    }
}
