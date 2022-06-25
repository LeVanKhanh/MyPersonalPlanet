namespace Mpp.Architecture.Core.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

public class DomainMapper : IDomainMapper
{
    private readonly IServiceProvider _serviceProvider;
    public DomainMapper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination? destination = null)
        where TDestination : class, new()
    {
        if (destination == null) destination = new TDestination();
        var mapper = _serviceProvider.GetRequiredService<IDomainMapper<TSource, TDestination>>();
        return mapper.Map(source, destination);
    }
}
