namespace Mpp.Architecture.Core.Infrastructure.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Mpp.Architecture.Core.Domain.DependencyInjection;
using System.Reflection;

public static class InfrastructureDependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddDomain(Assembly.GetExecutingAssembly());
    }
}
