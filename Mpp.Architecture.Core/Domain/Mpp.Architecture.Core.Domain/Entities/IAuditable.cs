namespace Mpp.Architecture.Core.Domain.Entities;

public interface IAuditable<T> : ISoftDelete
    where T : struct
{
    T CreatedBy { get; set; }
    DateTime CreatedDate { get; set; }
    T? UpdatedBy { get; set; }
    DateTime? UpdatedDate { get; set; }
}
