using System.Net;
using System.Text.Json;

namespace TodoApi.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (KeyNotFoundException knfe)
            {
                await HandleExceptionAsync(context, knfe, "The resource was not found.");
            }
            catch (ArgumentException anfe)
            {
                await HandleExceptionAsync(context, anfe, "A required attribute was null or empty.");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, "An unexpected error occurred.");
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, string message)
        {

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                Detailed = exception.Message
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }

}