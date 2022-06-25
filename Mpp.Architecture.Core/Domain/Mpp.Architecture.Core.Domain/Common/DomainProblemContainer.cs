namespace Mpp.Architecture.Core.Domain.Common;
public sealed class DomainProblemContainer
{
    private DomainProblemContainer() { }
    public static readonly DomainProblem BadEntity = new(5000, "The entity {0} is invalid. Validation error message: {1}");
    public static readonly DomainProblem EntityNotFound = new(5001, "The entity {0} is not found.");
}