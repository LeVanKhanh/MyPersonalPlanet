namespace Mpp.Architecture.Core.Infrastructure.MessageBroker;

using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;

public abstract class AzureServiceBus : IMessageBus
{
    private readonly IServiceBusPersisterConnection _persistentConnection;
    private const string IntegrationEventSuffix = "IntegrationEvent";
    private readonly IServiceProvider _serviceProvider;
    private readonly IQueueClient _queueClient;
    private readonly ConnectionModel _connectionModel;

    public AzureServiceBus(IServiceBusPersisterConnection persistentConnection
        , ConnectionModel connectionModel
        , IServiceProvider serviceProvider)
    {
        _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
        _queueClient = _persistentConnection.CreateModel();
        _connectionModel = connectionModel;
        _serviceProvider = serviceProvider;
    }

    public void Publish(Message message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        var eventName = message.GetType().Name.Replace(IntegrationEventSuffix, "");
        var jsonMessage = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        var sms = new Microsoft.Azure.ServiceBus.Message
        {
            Body = body,
            Label = eventName,
        };

        if (_connectionModel.IsDuplicateDetectionEnabled)
        {
            sms.MessageId = message.Id;
        }

        _queueClient.SendAsync(sms)
            .GetAwaiter()
            .GetResult();
    }

    public void Publish(IEnumerable<Message> events)
    {
        if (events == null || !events.Any())
        {
            throw new ArgumentNullException(nameof(events));
        }

        // There is a 256 KB limit per message sent on Azure Service Bus.
        // We will divide it into messages block lower or equal to 256 KB.
        // Maximum message size: 256 KB for Standard tier, 1 MB for Premium tier.
        // Maximum header size: 64 KB.
        // https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quotas
        var maxBatchSize = 256000 - 64000;
        var sumBodySize = 0;

        var smss = new List<Microsoft.Azure.ServiceBus.Message>();
        foreach (var @event in events)
        {
            var eventName = @event.GetType().Name.Replace(IntegrationEventSuffix, "");
            var jsonMessage = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var sms = new Microsoft.Azure.ServiceBus.Message
            {
                MessageId = @event.Id,
                Body = body,
                Label = eventName,
            };

            var bodySize = jsonMessage.Length * sizeof(char);
            sumBodySize += bodySize;
            if (sumBodySize > maxBatchSize)
            {
                _queueClient.SendAsync(smss)
                    .GetAwaiter()
                    .GetResult();

                smss.Clear();
                sumBodySize = bodySize;
            }

            smss.Add(sms);
        }

        if (smss.Any())
        {
            _queueClient.SendAsync(smss)
                        .GetAwaiter()
                        .GetResult();
        }
    }

    public void RegisterHandler<T, TH>()
        where T : Message
        where TH : IIntegrationEventHandler<T>
    {
        var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
        {
            // Maximum number of Concurrent calls to the callback `ProcessMessagesAsync`, set to 1 for simplicity.
            // Set it according to how many messages the application wants to process in parallel.
            MaxConcurrentCalls = 1,

            // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
            // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
            AutoComplete = false,

            // Set the duration for a message to kept by a consumer.
            // Why 5 minutes? It should be deduced from the biggest time difference between 2 continual exceptions
            // of type Microsoft.Azure.ServiceBus.MessageLockLostException observed on Azure Monitoring.
            MaxAutoRenewDuration = TimeSpan.FromMinutes(5),
        };

        if (_connectionModel.PrefetchCount > 1)
        {
            messageHandlerOptions.MaxConcurrentCalls = _connectionModel.PrefetchCount;
        }

        // Register the function that will process messages
        _queueClient.RegisterMessageHandler(ProcessMessagesAsync<T, TH>, messageHandlerOptions);
    }

    public async Task CompleteAsync(string messageToken)
    {
        await _queueClient.CompleteAsync(messageToken);
    }

    private async Task ProcessMessagesAsync<T, TH>(Microsoft.Azure.ServiceBus.Message message, CancellationToken token)
        where T : Message
        where TH : IIntegrationEventHandler<T>
    {
        T integrationEvent;
        try
        {
            integrationEvent = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));
        }
        catch
        {
            integrationEvent = Activator.CreateInstance<T>();
            integrationEvent.HasError = true;
        }

        if (integrationEvent != null)
        {
            integrationEvent.MessageToken = message.SystemProperties.LockToken;
        }

        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(typeof(T));

        using (var scope = _serviceProvider.CreateScope())
        {
            await Task.Yield();

            var handler = scope.ServiceProvider.GetService(typeof(TH));
            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
        }
    }

    private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
    {
        var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

        var errorBuilder = new StringBuilder();
        errorBuilder.AppendLine("Message handler encountered an exception: {exception}.");
        errorBuilder.AppendLine("Exception context for troubleshooting:");
        errorBuilder.AppendLine("- Endpoint: {endpoint}");
        errorBuilder.AppendLine("- Entity Path: {entityPath}");
        errorBuilder.AppendLine("- Executing Action: {action}");

        return Task.CompletedTask;
    }
}