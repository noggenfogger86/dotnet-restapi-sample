using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net;

namespace ApiSkeleton.Common
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(err =>
            {
                err.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        //logger.Error($"Something went wrong: {contextFeature.Error}");
                        if (contextFeature.Error is LogicException)
                        {
                            var logicException = contextFeature.Error as LogicException;
                            context.Response.StatusCode = (int)logicException.HttpStatusCode;

                            var errorDetails = new ErrorDetails
                            {
                                StatusCode = context.Response.StatusCode,
                                ErrorMessage = string.IsNullOrEmpty(logicException.Detail) ?
                                            logicException.ResultCode.GetDescription() :
                                            logicException.ResultCode.GetDescription() + System.Environment.NewLine + "[Detail] " + logicException.Detail,
                                Detail = logicException.Detail,
                                ResultCode = logicException.ResultCode
                            };

                            if (env.IsDevelopment())
                            {
                                errorDetails.StackTrace = contextFeature.Error.StackTrace;
                            }

                            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorDetails));
                        }
                        else
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                            var errorDetails = new ErrorDetails
                            {
                                StatusCode = context.Response.StatusCode,
                                ErrorMessage = "Unhandled Exception",
                                Detail = contextFeature.Error.Message,
                                ResultCode = ResultCode.UnknownException
                            };

                            if (env.IsDevelopment())
                            {
                                errorDetails.StackTrace = contextFeature.Error.StackTrace;
                            }

                            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorDetails));
                        }
                    }
                });
            });
        }
    }
}
