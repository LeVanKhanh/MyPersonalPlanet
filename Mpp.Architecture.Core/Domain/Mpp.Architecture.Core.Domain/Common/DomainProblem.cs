namespace Mpp.Architecture.Core.Domain.Common;

public struct DomainProblem
{
    public DomainProblem(int problemId, string message)
    {
        ProblemId = problemId;
        Message = message;
    }

    public int ProblemId { get; private set; }
    public string Message { get; private set; }
}
