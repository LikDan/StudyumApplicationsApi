namespace ApplicationsApi.Utils.Parser.Utils;

public class TimeoutException : Exception {
    public TimeoutException(int timeout) : base(_message(timeout)) { }
    private static string _message(int timeout) => $"It took to long to proceed application, limit is {timeout}ms";
}