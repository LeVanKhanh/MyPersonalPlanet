namespace Mpp.Architecture.Core.Domain.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection;
    using FluentValidation;
    using System.Reflection;
    using Mpp.Architecture.Core.Domain.Services;
    using MediatR;
    using Mpp.Architecture.Core.Domain.DomainBusinessHanlder;

    public static class DomainDependencyInjection
    {
        public static void AddDomain(this IServiceCollection services, Assembly assembly)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(assembly);
            services.AddSingleton<IValidatorFactory, ServiceProviderValidatorFactory>();
            services.AddSingleton<IDomainValidationService, DomainValidationService>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        }
    }
}
