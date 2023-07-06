using Cefalo.InfedgeBlog.Service.CustomExceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Cefalo.InfedgeBlog.Api.Utils.GlobalErrorHandler
{
    public static class GlobalErrorHandler
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {

                    context.Response.ContentType = "application/json";
                    var contextFeatures = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeatures != null)
                    {
                        Type type = contextFeatures.Error.GetType();
                        context.Response.StatusCode = GetStatusCode(type);
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeatures.Error.Message,
                        }.ToString());
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "An error occured. Please try again.",
                        }.ToString());
                    }
                });
            });
        }
        public static int GetStatusCode(Type type)
        {
            if (type == typeof(BadRequestException))
            {
                return (int)HttpStatusCode.BadRequest;
            }
            else if (type == typeof(UnauthorizedException))
            {
                return (int)HttpStatusCode.Unauthorized;
            }
            else if (type == typeof(NotFoundException))
            {
                return (int)HttpStatusCode.NotFound;
            }
            else
            {
                return (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}
