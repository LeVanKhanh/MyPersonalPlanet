namespace Mpp.Architecture.Core.Domain.DependencyInjection;

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Mpp.Architecture.Core.Domain.DomainBusinessHanlder;
using Mpp.Architecture.Core.Domain.Services;
using System.Reflection;

public static class DomainDependencyInjection
{
    public static void AddDomain(this IServiceCollection services, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(assembly);
        services.AddSingleton<IValidatorFactory, ServiceProviderValidatorFactory>();
        services.AddSingleton<IDomainValidationService, DomainValidationService>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped(typeof(IDomainMapper), typeof(DomainMapper));
    }
}