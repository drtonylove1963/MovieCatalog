using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MovieCatalog.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request != null)
        {
            var context = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(request, context, validationResults, true);

            if (!isValid)
            {
                var errors = validationResults
                    .Select(r => new { Property = r.MemberNames.FirstOrDefault() ?? string.Empty, Message = r.ErrorMessage })
                    .ToList();

                if (errors.Any())
                {
                    throw new ValidationException($"Validation failed: {string.Join(", ", errors.Select(e => $"{e.Property}: {e.Message}"))}");
                }
            }
        }

        return await next();
    }
}
