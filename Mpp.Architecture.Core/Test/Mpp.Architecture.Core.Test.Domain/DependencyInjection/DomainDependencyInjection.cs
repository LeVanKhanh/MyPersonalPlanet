namespace Mpp.Architecture.Core.Test.Domain.DependencyInjection
{
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Mpp.Architecture.Core.Domain.Services;
    using System.Reflection;

    public static class DomainDependencyInjection
    {
        public static void AddTestDomain(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddValidatorsFromAssembly(assembly);
            services.AddMediatR(assembly);
            assembly.GetTypes()
                .Where(item => item.GetInterfaces()
                .Where(i => i.IsGenericType).Any(i => i.GetGenericTypeDefinition() == typeof(IDomainMapper<,>)) && !item.IsAbstract && !item.IsInterface)
                .ToList()
                .ForEach(assignedTypes =>
                {
                    var serviceType = assignedTypes.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IDomainMapper<,>));
                    services.AddScoped(serviceType, assignedTypes);
                });
        }
    }
}
