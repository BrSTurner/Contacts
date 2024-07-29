using FluentValidation;
using FluentValidation.Results;

namespace FIAP.Contacts.Application.Services
{
    public abstract class BaseAppService
    {
        public BaseAppService() { }

        public async Task EnsureValidationAsync<TInput, TValidator>(TInput input)
            where TValidator : AbstractValidator<TInput>
        {
            var validationResult = await IsValidAsync<TInput, TValidator>(input);
            EnsureValidationResult(validationResult);
        }


        public void EnsureValidation<TInput, TValidator>(TInput input)
            where TValidator : AbstractValidator<TInput>
        {
            var validationResult = IsValid<TInput, TValidator>(input);
            EnsureValidationResult(validationResult);
        }

        public ValidationResult IsValid<TInput, TValidator>(TInput input) 
            where TValidator : AbstractValidator<TInput>
        {
            var validator = Activator.CreateInstance<TValidator>();
            return validator.Validate(input);
        }

        public Task<ValidationResult> IsValidAsync<TInput, TValidator>(TInput input)
            where TValidator : AbstractValidator<TInput>
        {
            var validator = Activator.CreateInstance<TValidator>();
            return validator.ValidateAsync(input);
        }

        private void EnsureValidationResult(ValidationResult result)
        {
            if (!result.IsValid)
                throw new Exception($"{result.Errors.First().ErrorMessage}");
        } 
    }
}
