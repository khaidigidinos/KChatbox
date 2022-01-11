using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using SignalRApi.Services;

namespace SignalRApi.Pipelines
{
    public class LoggingProcess<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly CurrentUserService _currentUSerService;

        public LoggingProcess(ILogger<TRequest> logger, CurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUSerService = currentUserService;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken = default)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUSerService.CurrentUserId() ?? "";
            var userEmail = _currentUSerService.CurrentUserEmail() ?? "";

            _logger.LogInformation($"Request: {requestName} from type {typeof(TRequest)}. User id: {userId}, user email: {userEmail}");
            return Task.CompletedTask;
        }
    }
}
