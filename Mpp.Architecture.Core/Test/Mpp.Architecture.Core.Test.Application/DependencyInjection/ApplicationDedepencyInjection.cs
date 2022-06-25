namespace Mpp.Architecture.Core.Test.Application.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection;
    using Mpp.Architecture.Core.Application.DependencyInjection;
    using Mpp.Architecture.Core.Test.Infrastructure.DependencyInjection;

    public static class ApplicationDedepencyInjection
    {
        public static void AddTestApplication(this IServiceCollection services)
        {
            services.AddApplication();
            services.AddTestInfrastructure();
        }
    }
}
