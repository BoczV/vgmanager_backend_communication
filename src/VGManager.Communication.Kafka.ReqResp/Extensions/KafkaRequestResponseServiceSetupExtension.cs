using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VGManager.Communication.Kafka.Extensions;
using VGManager.Communication.Kafka.RequestResponse.HostedServices;
using VGManager.Communication.Kafka.RequestResponse.Interfaces;
using VGManager.Communication.Models;
using VGManager.Communication.Models.Interfaces;

namespace VGManager.Communication.Kafka.RequestResponse.Extensions;

public static class KafkaRequestResponseServiceSetupExtension
{
    public static void SetupKafkaRequestResponse<TRequest, TResponse>(
        this IServiceCollection services,
        IConfiguration configuration,
        string kafkaProducerSectionKey,
        string kafkaConsumerSectionKey)
        where TRequest : MessageBase, ICommandRequest
        where TResponse : MessageBase, ICommandResponse
    {
        services.AddSingleton<IRequestStoreService<TResponse>, RequestStoreService<TResponse>>();

        services.SetupKafkaConsumer<TResponse>(configuration, kafkaConsumerSectionKey, true);
        services.SetupKafkaProducer<TRequest>(configuration, kafkaProducerSectionKey);

        services.AddScoped<IKafkaRequestResponseService<TRequest, TResponse>, KafkaRequestResponseService<TRequest, TResponse>>();

        services.AddHostedService<KafkaResponseHostedService<TResponse>>();

    }
}
