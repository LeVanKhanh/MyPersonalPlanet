namespace Mpp.Architecture.Core.Domain.DomainBusinessHanlder;
using Mpp.Architecture.Core.Domain.Common;

public interface IDomainResult<T>
{
    bool Success { get; }
    List<DomainProblem>? DomainProblems { get;}
    T? Result { get; }
}