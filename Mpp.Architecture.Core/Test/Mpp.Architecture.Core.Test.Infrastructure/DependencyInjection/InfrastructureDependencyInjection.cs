namespace Mpp.Architecture.Core.Test.Infrastructure.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection;
    using Mpp.Architecture.Core.Test.Domain.DependencyInjection;
    using Mpp.Architecture.Core.Test.Domain;
    using Mpp.Architecture.Core.Test.Infrastructure.Persistance;

    public static class InfrastructureDependencyInjection
    {
        public static void AddTestInfrastructure(this IServiceCollection services)
        {
            services.AddTestDomain();
            services.AddScoped<IOrderRepository, OrderRepository>();
        }
    }
}
