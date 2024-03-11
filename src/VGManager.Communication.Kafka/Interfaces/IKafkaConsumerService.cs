namespace VGManager.Communication.Kafka.Interfaces;

public interface IKafkaConsumerService<out TMessageType> : IDisposable
{
    Task ConsumeAsync(Func<TMessageType, Task> handlerMethod, CancellationToken cancellationToken);
    Task ConsumeSequentiallyAsync(Func<TMessageType, Task> handlerMethod, CancellationToken cancellationToken);
}
