namespace Mpp.Architecture.Core.Domain.Common;

using System.Runtime.Serialization;

[Serializable]
public class DomainException : Exception
{
    public int ErrorCode { get; set; }

    public DomainException(DomainProblem domainProblem)
        : this(domainProblem.Message, domainProblem.ProblemId)
    {

    }

    public DomainException(DomainProblem domainProblem, params object?[] pars)
        : this(string.Format(domainProblem.Message, pars), domainProblem.ProblemId)
    {

    }

    public DomainException(string message, int errorCode)
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public DomainException(string message, int errorCode, Exception? innerException)
    : base(message, innerException)
    {
        ErrorCode = errorCode;
    }

    protected DomainException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {

    }
}
