namespace Mpp.Architecture.Core.Domain.DomainBusinessHanlder;
using Mpp.Architecture.Core.Domain.Common;

public class DomainResult<T> : IDomainResult<T>
{
    public DomainResult(T? result)
    {
        Result = result;
        Success = true;
    }

    public DomainResult(List<DomainProblem> domainProblems)
    {
        Success = false;
        DomainProblems = domainProblems;
    }

    public bool Success { get; private set; }
    public T? Result { get; private set; }
    public List<DomainProblem>? DomainProblems { get; private set; }
}