using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VGManager.Communication.Kafka.Interfaces;
using VGManager.Communication.Kafka.RequestResponse.Interfaces;
using VGManager.Communication.Models.Interfaces;

namespace VGManager.Communication.Kafka.RequestResponse.HostedServices;

public class KafkaResponseHostedService<TResponse> : BackgroundService where TResponse : ICommandResponse
{
    private readonly IKafkaConsumerService<TResponse> _consumerService;
    private readonly IRequestStoreService<TResponse> _store;
    private readonly ILogger<KafkaResponseHostedService<TResponse>> _logger;

    public KafkaResponseHostedService(
        IKafkaConsumerService<TResponse> consumerService,
        IRequestStoreService<TResponse> store,
        ILogger<KafkaResponseHostedService<TResponse>> logger)
    {
        _consumerService = consumerService;
        _store = store;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumerService.ConsumeAsync((message) =>
        {
            var task = _store.GetAndRemove(message.CommandInstanceId);
            if (task is not null)
            {
                _logger.LogDebug("Message with id: {Id} was identified as a response message. Processing message.", message.CommandInstanceId);
                _ = task.TrySetResult(message);
            }
            else
            {
                _logger.LogDebug("Message with id: {Id} was not identified as a response message. Skipping process.", message.CommandInstanceId);
            }
            return Task.CompletedTask;
        }, stoppingToken);
    }
}
