namespace Mpp.Architecture.Core.Domain.Services;

using FluentValidation.Results;
using Mpp.Architecture.Core.Domain.Entities;

public interface IDomainValidationService
{
    ValidationResult Validate<T>(T entity) where T : Entity;
}
