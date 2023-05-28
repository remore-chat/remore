using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Remore.Shared.PacketHandler
{
    public class PacketHandler
    {
        private Dictionary<string, Type> _map;

        public PacketHandler(Type type)
        {
            Type = type;
            InitializeTypesMap();
        }

        private void InitializeTypesMap()
        {
            _map = typeof(IPacket)
                .Assembly
                .GetTypes()
                .Where(x => x.IsAssignableFrom(typeof(IPacket)))
                .ToDictionary(x => ((IPacket)Activator.CreateInstance(x)).PacketId, x => x.DeclaringType);

        }

        public Type Type { get; }

        public void Handle(string json, Dictionary<string, object> metadata = null)
        {
            try
            {
                var ipacket = JsonSerializer.Deserialize<IPacket>(json);
                if (!_map.TryGetValue(ipacket.PacketId, out var type)) return;

                var packet = JsonSerializer.Deserialize(json, type);

                if (!(packet as IPacket).CheckIfPacketValid())
                {
                    throw new Exception("Invalid packet");
                }
                var HandleMethod = type.GetMethod("Handle" + type.Name);
                if (HandleMethod != null)
                {
                    var instance = Activator.CreateInstance(type);
                    var meta = type.GetProperty("Metadata");
                    if (meta != null)
                        meta.SetValue(instance, metadata);
                    HandleMethod.Invoke(instance, new object[] { packet });
                }

            }
            catch (Exception ex)
            {
                //Ignore invalid packet;
            }

        }
    }
}
