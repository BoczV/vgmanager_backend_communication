using Confluent.Kafka;

namespace VGManager.Communication.Kafka.Interfaces;

public interface IMessageSerializer<T> : IDeserializer<T>, ISerializer<T>
{
}
