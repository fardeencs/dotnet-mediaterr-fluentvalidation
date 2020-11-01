using FluentValidation.Results;

namespace WebApi.Infrastructure.Processes
{
    public interface IValidator<in T>
    {
        ValidationResult Validate(T instance);
    }
}