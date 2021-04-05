
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Expections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI
{
    public class ErrorHandlingMiddleware:IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
              await next.Invoke(context);
            }
            catch (ForbidException forbidExecption)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(forbidExecption.Message);

            }
            catch (BadHttpRequestException badrequest)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(badrequest.Message);
            }
            catch (NotFoundExpection notfound)
            {
                context.Response.StatusCode = 404;
                 await context.Response.WriteAsync(notfound.Message);
            }
            catch(Exception e)
            {
                logger.LogError(e,e.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong");
            }

          
            
        }
    }
}
