using System.Net;

namespace ApplicationsApi.ExceptionHandling; 

public class HttpException : Exception {
    public override string Message { get; }
    public int Code { get; }

    public HttpException(string message, int code) {
        Message = message;
        Code = code;
    }

    public HttpException(string message, HttpStatusCode code) : this(message, (int)code) {}
}