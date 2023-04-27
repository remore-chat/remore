using Remore.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;

namespace Remore.Server.Models
{
    public class RemoreServerClient
    {
        public ClientMetadata ClientMetadata { get; }
        public RemorePublicClient RemoreClient { get; set; }

        public RemoreServerClient(ClientMetadata clientMetadata, RemorePublicClient remoreClient)
        {
            ClientMetadata = clientMetadata;
            RemoreClient = remoreClient;
        }

    }
}
