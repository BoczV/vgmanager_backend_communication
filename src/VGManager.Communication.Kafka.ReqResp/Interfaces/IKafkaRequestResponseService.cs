using VGManager.Communication.Models.Interfaces;

namespace VGManager.Communication.Kafka.RequestResponse.Interfaces;

public interface IKafkaRequestResponseService<TRequest, TResponse>
    where TRequest : ICommandRequest
    where TResponse : class
{
    Task<TResponse?> SendAndReceiveAsync(TRequest request, CancellationToken cancellationToken = default);
}
