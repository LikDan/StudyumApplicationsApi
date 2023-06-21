using System.Net;
using ApplicationsApi.Utils.Validators.Utils;

namespace ApplicationsApi.ExceptionHandling;

public class ExceptionHandlerMiddleware : IMiddleware {
    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        try {
            try {
                await next(context);
            }
            catch (ValidationException e) {
                throw new HttpException(e.Message, HttpStatusCode.UnprocessableEntity);
            }
            catch (Exception e) {
                Console.WriteLine(e.GetType().ToString());
                Console.WriteLine(e.Message);
                throw new HttpException(e.Message, HttpStatusCode.InternalServerError);
            }
        }
        catch (HttpException e) {
            ProceedHttpException(context, e);
        }
    }

    private static async void ProceedHttpException(HttpContext context, HttpException exception) {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = exception.Code;
        await response.WriteAsync(exception.Message);
    }
}