using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SignalRApi.Exceptions;
using SignalRApi.Services;

namespace SignalRApi.Pipelines
{
    public class AuthorizePipeline<TRequest, TResponse> : IPipelineBehavior<TRequest,TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IAuthorizationPolicyProvider _authorizationPolicyProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationService _authorizationService;
        private readonly CurrentUserService _currentUserService;

        public AuthorizePipeline(ILogger<TRequest> logger, IAuthorizationPolicyProvider authorizationPolicyProvider
            , IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService, CurrentUserService currentUserService)
        {
            _logger = logger;
            _authorizationPolicyProvider = authorizationPolicyProvider;
            _httpContextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            /* TODO Can create ResourceAuthorizeAttribute */
            var authorizeAttr = request.GetType().GetCustomAttribute<AuthorizeAttribute>();

            if (authorizeAttr is not null)
            {
                if (_currentUserService.CurrentUserId() is null)
                {
                    throw new UnauthorizedAccessException("Unauthorized. There is no user information.");
                }

                var policy = authorizeAttr.Policy ?? null;
                if (policy is not null)
                {
                    var appliedPolicy = await _authorizationPolicyProvider.GetPolicyAsync(policy);
                    if (appliedPolicy is null)
                    {
                        throw new NullReferenceException($"Policy: \"{policy}\" does not exist. Exception occured at {nameof(Handle)} in type of {typeof(AuthorizePipeline<TRequest, TResponse>)}");
                    }
                    var requirementsList = appliedPolicy.Requirements.ToList();
                    var result = await _authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, null, requirementsList);
                    if (!result.Succeeded)
                    {
                        throw new UnauthorizedAccessException($"Unauthorized. Current user does not meet the requirements of the \"{policy}\" policy.");
                    }
                }

                var roles = authorizeAttr.Roles ?? null;
                if (roles is not null)
                {
                    var anyAvailableRole = roles
                        .Split(",")
                        .Any(role => _currentUserService.CurrentUserRoles().Contains(role));
                    if (!anyAvailableRole)
                    {
                        throw new UnauthorizedAccessException($"Unauthorized. Current user does not match one of the specific roles: {roles.Split(",").Aggregate((prev, next) => prev + ", " + next)}.");
                    }
                }
            }

            return await next();
        }
    }
}
