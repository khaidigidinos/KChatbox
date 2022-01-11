using System;
using Microsoft.Extensions.Logging;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace SignalRApi.Pipelines
{
    public class TimingRequestPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly Stopwatch _timer; 

        public TimingRequestPipeline(ILogger<TRequest> logger)
        {
            _logger = logger;
            _timer = new Stopwatch();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = typeof(TRequest).Name;
            _timer.Start();
            var response = await next();
            _timer.Stop();
            _logger.LogInformation($"Request: {requestName} from type: {typeof(TRequest)}. Time taken: {_timer.ElapsedMilliseconds} milliseconds.");
            return response;
        }
    }
}
