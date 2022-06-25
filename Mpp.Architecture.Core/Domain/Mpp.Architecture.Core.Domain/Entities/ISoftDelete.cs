namespace Mpp.Architecture.Core.Domain.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; }
    }
}
