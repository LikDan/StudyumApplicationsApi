using System.Net;
using ApplicationsApi.Utils.Validators.Utils;
using Grpc.Core;
using TimeoutException = ApplicationsApi.Utils.Parser.Utils.TimeoutException;

namespace ApplicationsApi.ExceptionHandling;

public class ExceptionHandlerMiddleware : IMiddleware {
    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        try {
            try {
                await next(context);
            }
            catch (RpcException e) {
                var code = e.StatusCode switch {
                    StatusCode.OK => HttpStatusCode.OK,
                    StatusCode.Cancelled => HttpStatusCode.BadRequest,
                    StatusCode.Unknown => HttpStatusCode.InternalServerError,
                    StatusCode.InvalidArgument => HttpStatusCode.BadRequest,
                    StatusCode.DeadlineExceeded => HttpStatusCode.GatewayTimeout,
                    StatusCode.NotFound => HttpStatusCode.NotFound,
                    StatusCode.AlreadyExists => HttpStatusCode.Conflict,
                    StatusCode.PermissionDenied => HttpStatusCode.Forbidden,
                    StatusCode.Unauthenticated => HttpStatusCode.Unauthorized,
                    StatusCode.ResourceExhausted => HttpStatusCode.ServiceUnavailable,
                    StatusCode.FailedPrecondition => HttpStatusCode.BadRequest,
                    StatusCode.Aborted => HttpStatusCode.Conflict,
                    StatusCode.OutOfRange => HttpStatusCode.BadRequest,
                    StatusCode.Unimplemented => HttpStatusCode.NotImplemented,
                    StatusCode.Internal => HttpStatusCode.InternalServerError,
                    StatusCode.Unavailable => HttpStatusCode.ServiceUnavailable,
                    StatusCode.DataLoss => HttpStatusCode.InternalServerError,
                    _ => HttpStatusCode.InternalServerError
                };
                throw new HttpException(e.Status.Detail, code);
            }
            catch (ValidationException e) {
                throw new HttpException(e.Message, HttpStatusCode.UnprocessableEntity);
            }
            catch (TimeoutException e) {
                throw new HttpException(e.Message, HttpStatusCode.RequestTimeout);
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