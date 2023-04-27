using System;
using System.Collections.Generic;
using System.Text;

namespace Remore.Shared.Models
{
    //Represents Remore client without sensitive information 
    public class RemorePublicClient
    {
        public RemorePublicClient()
        {

        }
        public RemorePublicClient(string name, Guid guid)
        {
            Name = name;
            Guid = guid;
        }

        public string Name { get; }
        public Guid Guid { get; }
    }
}
