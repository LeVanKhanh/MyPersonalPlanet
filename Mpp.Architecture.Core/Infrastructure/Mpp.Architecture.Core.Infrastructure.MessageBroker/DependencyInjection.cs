namespace Mpp.Architecture.Core.Infrastructure.MessageBroker;

using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterEventBus<TI, TRMQ, TASB>(this IServiceCollection services, ConnectionModel connectionModel)
        where TI : IMessageBus
        where TRMQ : RabbitMqBus, TI
        where TASB : AzureServiceBus, TI
    {
        if (connectionModel.AzureSBEnabled)
        {
            services.AddSingleton(typeof(TI), sp =>
            {
                var connectionStringBuilder = new ServiceBusConnectionStringBuilder(connectionModel.AzureSBConnectionString)
                {
                    EntityPath = connectionModel.QueueName,
                    TransportType = TransportType.AmqpWebSockets
                };
                var persistentConnection = new DefaultServiceBusPersisterConnection(connectionStringBuilder);

                var constructorInfo = typeof(TASB).GetConstructor(new[]
                {
                        persistentConnection.GetType(),
                        connectionModel.GetType(),
                        sp.GetType(),
                    });
                return (TASB)constructorInfo.Invoke(new object[] { persistentConnection, connectionModel, sp });
            });
        }
        else
        {
            services.AddSingleton(typeof(TI), sp =>
            {
                var connectionFactory = new ConnectionFactory
                {
                    HostName = connectionModel.RabbitMQConnection,
                    Port = connectionModel.RabbitMQConnectionPort,
                    UserName = connectionModel.RabbitMQUserName,
                    Password = connectionModel.RabbitMQPassword,
                };
                var persistentConnection = new DefaultRabbitMQPersistentConnection(connectionFactory, connectionModel.QueueName);

                var constructorInfo = typeof(TRMQ).GetConstructor(new[]
                {
                        persistentConnection.GetType(),
                        connectionModel.GetType(),
                        sp.GetType(),
                    });
                return (TRMQ)constructorInfo.Invoke(new object[] { persistentConnection, connectionModel, sp });
            });
        }

        return services;
    }
}