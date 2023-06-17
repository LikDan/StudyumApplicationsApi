using System.Net.WebSockets;

namespace ApplicationsApi.Utils.Websockets;

public abstract class ImplHttpWebSockets<TR, TS> : ImplWebSockets<TR, TS> {
    public override WebSocket WebSocket { get; }

    protected ImplHttpWebSockets(HttpContext context) {
        WebSocket = context.WebSockets.AcceptWebSocketAsync().Result;
    }
}

// ReSharper disable once UnusedType.Global
public abstract class ImplHttpWebSockets<T> : ImplHttpWebSockets<T, T> {
    protected ImplHttpWebSockets(HttpContext context) : base(context) { }
}