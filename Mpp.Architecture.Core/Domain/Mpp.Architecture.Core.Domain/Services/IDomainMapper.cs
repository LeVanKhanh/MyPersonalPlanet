namespace Mpp.Architecture.Core.Domain.Services;

public interface IDomainMapper
{
    TDestination Map<TSource, TDestination>(TSource source);
    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
}