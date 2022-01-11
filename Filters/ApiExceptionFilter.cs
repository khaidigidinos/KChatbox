using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SignalRApi.Exceptions;

namespace SignalRApi.Filters
{
    public class ApiExceptionFilter : IAsyncExceptionFilter
    {
        private readonly Dictionary<Type, Action<ExceptionContext>> _dict;

        public ApiExceptionFilter()
        {
            _dict = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(NotFoundException), HandleNotFoundError },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                { typeof(ValidationException), HandleValidationException }
            };
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            await CallCorespodingHanlder(context);
        }

        private Task CallCorespodingHanlder(ExceptionContext context)
        {
            var errorType = context.Exception.GetType();

            if (_dict.ContainsKey(errorType))
            {
                _dict[errorType].Invoke(context);
            }
            else
            {
                HandleUnknownException(context);
            }

            return Task.CompletedTask;
        }

        private void HandleNotFoundError(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;

            context.Result = new ObjectResult(new {
                data = (string) null,
                status = StatusCodes.Status404NotFound,
                message = exception.Message
            });

            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        }

        private void HandleUnauthorizedAccessException(ExceptionContext context)
        {
            var exception = context.Exception as UnauthorizedAccessException;

            context.Result = new ObjectResult(new
            {
                data = (string)null,
                status = StatusCodes.Status401Unauthorized,
                message = exception.Message
            });

            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            var exception = context.Exception;

            context.Result = new ObjectResult(new {
                data = (string) null,
                status = StatusCodes.Status500InternalServerError,
                message = exception.Message
            });

            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        private void HandleValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;

            context.Result = new ObjectResult(new
            {
                data = (string)null,
                status = StatusCodes.Status400BadRequest,
                message = exception.ErrorDict
            });

            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
