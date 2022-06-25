namespace Mpp.Architecture.Core.Test.Domain;

using Microsoft.Extensions.DependencyInjection;
using Mpp.Architecture.Core.Domain.Services;

public class DomainMapper : IDomainMapper
{
    private readonly IServiceProvider _serviceProvider;
    public DomainMapper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public TDestination Map<TSource, TDestination>(TSource source)
    {
        var mapper = _serviceProvider.GetRequiredService<IDomainMapper<TSource, TDestination>>();
        return mapper.Map(source);
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        var mapper = _serviceProvider.GetRequiredService<IDomainMapper<TSource, TDestination>>();
        return mapper.Map(source);
    }
}

public interface IDomainMapper<in TSource, TDestination>
{
    TDestination Map(TSource source);
    TDestination Map(TSource source, TDestination destination);
}

public abstract class DomainMapper<TSource, TDestination> : IDomainMapper<TSource, TDestination>
{
    public abstract TDestination Map(TSource source);

    public abstract TDestination Map(TSource source, TDestination destination);
}
