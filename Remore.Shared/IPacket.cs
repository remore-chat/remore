using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remore.Shared
{
    public interface IPacket
    {
        public string PacketId { get; }


        public bool CheckIfPacketValid()
        {
            var props = this.GetType().GetProperties();
            var nonNullableProps = props.Where(x => Nullable.GetUnderlyingType(x.PropertyType) != null);
            return !nonNullableProps.Any(x => x.GetValue(this) == null);
        }
    }
}
