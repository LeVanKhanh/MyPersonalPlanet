namespace Mpp.Architecture.Core.Application.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Mpp.Architecture.Core.Infrastructure.DependencyInjection;

public static class ApplicationDedepencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddInfrastructure();
    }
}
