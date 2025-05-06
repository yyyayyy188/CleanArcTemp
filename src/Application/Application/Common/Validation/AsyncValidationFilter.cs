using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Mvc.Filters;

public class AsyncValidationFilter : IAsyncActionFilter
{
    private readonly ICustomValidatorFactory _validatorFactory;

    public AsyncValidationFilter(ICustomValidatorFactory validatorFactory)
    {
        _validatorFactory = validatorFactory;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Check if the action has the SkipAsyncValidationAttribute
        var hasSkip = context.ActionDescriptor.EndpointMetadata
               .Any(m => m is SkipAsyncValidationAttribute);
        if (hasSkip)
        {
            await next();
            return;
        }

        if (!context.ActionArguments.Any())
        {
            await next();
            return;
        }

        var validationFailures = new List<ValidationFailure>();

        foreach (var actionArgument in context.ActionArguments)
        {
            var validationErrors = await GetValidationErrorsAsync(actionArgument.Value!);
            validationFailures.AddRange(validationErrors);
        }

        if (!validationFailures.Any())
        {
            await next();
            return;
        }
        await next();
        // context.Result = new BadRequestObjectResult(validationFailures);
    }

    private async Task<IEnumerable<ValidationFailure>> GetValidationErrorsAsync(object value)
    {

        if (value == null)
        {
            return new[] { new ValidationFailure("", "instance is null") };
        }

        var validatorInstance = _validatorFactory.GetValidatorFor(value.GetType());
        if (validatorInstance == null)
        {
            return new List<ValidationFailure>();
        }

        var validationResult = await validatorInstance.ValidateAsync(new ValidationContext<object>(value));
        return validationResult.Errors ?? new List<ValidationFailure>();
    }
}