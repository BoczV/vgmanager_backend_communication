using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VGManager.Communication.Kafka.Configurations;
using VGManager.Communication.Kafka.Helper;
using VGManager.Communication.Kafka.Interfaces;
using VGManager.Communication.Models;

namespace VGManager.Communication.Kafka.Extensions;

public static class KafkaProducerSetupExtension
{
    public static void SetupKafkaProducer<TMessageType>(
        this IServiceCollection services,
        IConfiguration configuration,
        string kafkaProducerSectionKey)
        where TMessageType : MessageBase
    {
        var producerConfig = configuration.GetSection(kafkaProducerSectionKey)
           .Get<KafkaProducerConfiguration<TMessageType>>();

        services.AddSingleton(serviceProvider =>
        {
            return new ProducerBuilder<Null, TMessageType>(producerConfig.ProducerConfig)
            .SetValueSerializer(new MessageSerializer<TMessageType>())
            .SetLogHandler(LogHandler<TMessageType>(serviceProvider))
            .Build();
        });

        services.AddSingleton(producerConfig);
        services.AddSingleton<IKafkaProducerService<TMessageType>, KafkaProducerService<TMessageType>>();
    }

    private static Action<IProducer<Null, TMessageType>, LogMessage> LogHandler<TMessageType>(IServiceProvider serviceProvider) where TMessageType : MessageBase
    {
        return (producer, logMessage) =>
        {
            var logger = serviceProvider.GetRequiredService<ILogger<KafkaProducerService<TMessageType>>>();
            LogHelper.Log(logMessage, logger);
        };
    }
}
