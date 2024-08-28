using Domain.Primitives;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Behaviors;
internal sealed class ValidationBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }

        ValidationContext<TRequest> context = new(request);

        ValidationResult[] validationResult = await Task
            .WhenAll(validators.Select(v => v.ValidateAsync(context)));

        List<ValidationError> failures = validationResult
           .SelectMany(result => result.Errors)
           .Where(f => f != null)
           .Select(fl => new ValidationError(fl.PropertyName, fl.ErrorMessage))
           .Distinct()
           .ToList();

        if (failures.Count != 0)
        {
            return (TResponse)Result.ValidationError(failures);
        }

        return await next();
    }
}
