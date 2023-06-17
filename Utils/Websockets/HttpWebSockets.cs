using ApplicationsApi.Utils.Converters.Serializers;

namespace ApplicationsApi.Utils.Websockets;

// ReSharper disable UnusedMember.Global
public static class HttpWebSockets {
    public static IWebsocket<string, string> String(HttpContext context) => 
        new HttpWebSocket<string>(context, new StringByteSerializer());

    public static IWebsocket<T, T> Json<T>(HttpContext context) => 
        new HttpWebSocket<T>(context, new JsonByteSerializer<T>());
}