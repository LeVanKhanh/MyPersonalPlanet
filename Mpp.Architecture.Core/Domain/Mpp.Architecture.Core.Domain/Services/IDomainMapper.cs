namespace Mpp.Architecture.Core.Domain.Services;

public interface IDomainMapper
{
    TDestination Map<TSource, TDestination>(TSource source, TDestination? destination = null)
         where TDestination : class, new();
}

public interface IDomainMapper<in TSource, TDestination>
    where TDestination : class
{
    TDestination Map(TSource source, TDestination destination);
}