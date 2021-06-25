using API.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Common.Extensions
{
    public static class CustomErrorHandlerExtensions
    {
        public static void UseCustomErrors(this IApplicationBuilder app, IHostEnvironment environment, ILogger logger, bool isApi)
        {
            //if (environment.IsDevelopment())
            //{
            //    app.Use(new CustomErrorHandler(logger).WriteDevelopmentResponse);
            //}
            //else
            //{
            //    app.Use(new CustomErrorHandler(logger).WriteProductionResponse);
            //}
            app.Use(new CustomErrorHandler(logger, isApi).WriteDevelopmentResponse);
        }
    }
}
