using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ValidationException = SignalRApi.Exceptions.ValidationException;

namespace SignalRApi.Pipelines
{
    public class ValidationRequestPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validator;

        public ValidationRequestPipeline(IEnumerable<IValidator<TRequest>> validator)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validator.Any())
            {
                var allFailed = await Task.WhenAll(_validator.Select(async error => await error.ValidateAsync(request)));
                var errorDict = allFailed.Where(result => !result.IsValid)
                    .Select(result => result.Errors)
                    .SelectMany(eachFailed => eachFailed.Select(error => new { error.PropertyName, error.ErrorMessage }))
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(gr => gr.Key, gr => gr.Select(error => error.ErrorMessage).ToList());

                if (errorDict.Any())
                {
                    throw new ValidationException(errorDict);
                }
            }
            return await next();
        }
    }
}
