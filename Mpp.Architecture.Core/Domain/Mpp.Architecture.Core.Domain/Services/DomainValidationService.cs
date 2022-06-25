namespace Mpp.Architecture.Core.Domain.Services
{
    using FluentValidation;
    using FluentValidation.Results;
    using Mpp.Architecture.Core.Domain.Entities;

    public class DomainValidationService : IDomainValidationService
    {
        private readonly IValidatorFactory validatorFactory;

        public DomainValidationService(IValidatorFactory validatorFactory)
        {
            this.validatorFactory = validatorFactory;
        }

        public ValidationResult Validate<T>(T entity)
            where T : Entity
        {
            var validator = validatorFactory.GetValidator<T>();
            if (validator == null) return new ValidationResult();
            var result = validator.Validate(entity);
            return result;
        }
    }
}
