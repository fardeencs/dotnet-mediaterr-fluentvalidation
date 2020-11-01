using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace WebApi.Infrastructure.Handlers.Validation
{
    public class ValidatorHandler<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse> where TRequest : IAsyncRequest<TResponse>
    {
        private readonly IAsyncRequestHandler<TRequest, TResponse> inner;
        private readonly IValidator<TRequest>[] validators;

        public ValidatorHandler(IAsyncRequestHandler<TRequest, TResponse> inner, FluentValidation.IValidator<TRequest>[] validators)
        {
            this.inner = inner;
            this.validators = validators;
        }

        public Task<TResponse> Handle(TRequest message)
        {
            var context = new ValidationContext(message);

            var failures = validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return inner.Handle(message);
        }
    }
}