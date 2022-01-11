using System;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using SignalRApi.Pipelines;

namespace SignalRApi
{
    public static class AddMediaTrPipelines
    {
        public static IServiceCollection AddPipelines(this IServiceCollection services)
        {
            /* Add more pipelines if necessary */
            services.AddScoped(typeof(IRequestPreProcessor<>), typeof(LoggingProcess<>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionPipeline<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TimingRequestPipeline<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizePipeline<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationRequestPipeline<,>));
            return services;
        }
    }
}
