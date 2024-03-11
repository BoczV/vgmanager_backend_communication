using Confluent.Kafka;
using Microsoft.Extensions.Logging;
namespace VGManager.Communication.Kafka.Helper;

public static class LogHelper
{
    private const string LogMessageTemplate = "{@KafkaLogMessage}";

    public static void Log(LogMessage logMessage, ILogger logger)
    {
        switch (logMessage.Level)
        {
            case SyslogLevel.Emergency:
            case SyslogLevel.Alert:
            case SyslogLevel.Critical:
                logger.LogCritical(LogMessageTemplate, logMessage);
                break;
            case SyslogLevel.Error:
                logger.LogError(LogMessageTemplate, logMessage);
                break;
            case SyslogLevel.Warning:
                logger.LogWarning(LogMessageTemplate, logMessage);
                break;
            case SyslogLevel.Notice:
            case SyslogLevel.Info:
                logger.LogInformation(LogMessageTemplate, logMessage);
                break;
            case SyslogLevel.Debug:
                logger.LogDebug(LogMessageTemplate, logMessage);
                break;
        }
    }
}
