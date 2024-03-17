namespace Mpp.Architecture.Core.Infrastructure.MessageBroker;

public class ConnectionModel
{
    public bool AzureSBEnabled { get; set; }
    public string AzureSBConnectionString { get; set; }

    public string RabbitMQConnection { get; set; }
    public int RabbitMQConnectionPort { get; set; }
    public string RabbitMQUserName { get; set; }
    public string RabbitMQPassword { get; set; }

    public string QueueName { get; set; }
    public int PrefetchCount { get; set; }
    public bool IsDuplicateDetectionEnabled { get; set; }
}