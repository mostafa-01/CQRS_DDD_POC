using Confluent.Kafka;
using System.Text.Json;

namespace CQRSAndDDD_POC.Kafka
{
    public class EventSerialization
    {
        public class EventDeserializer<EventModel> : IDeserializer<EventModel?>
        {
            public EventModel? Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
            {
                return JsonSerializer.Deserialize<EventModel>(data);
            }
        }

        public class EventSerializer<EventModel> : ISerializer<EventModel>
        {
            public byte[] Serialize(EventModel data, SerializationContext context)
            {
                return JsonSerializer.SerializeToUtf8Bytes(data);
            }
        }
        public static class EventUtil
        {
            public static T? MapPayload<T>(object payload)
            {
                return JsonSerializer.Deserialize<T>((JsonElement)payload);
            }
        }

    }
}
