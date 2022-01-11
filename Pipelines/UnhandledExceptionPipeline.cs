using System;
using Microsoft.Extensions.Logging;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace SignalRApi.Pipelines
{
    public class UnhandledExceptionPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionPipeline(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch(Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, $"Request: {requestName} from type: {typeof(TRequest)}. Caught an exception at {DateTime.Now}");
                throw;
            }
        }
    }
}
