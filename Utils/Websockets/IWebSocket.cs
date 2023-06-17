using System.Net.WebSockets;

namespace ApplicationsApi.Utils.Websockets;

public interface IWebsocket<TR, in TS> {
    // ReSharper disable once UnusedMemberInSuper.Global
    public WebSocket WebSocket { get; }

    public Action<TR>? OnReceive { set; get; }

    public void StartReceiving();
    public void Send(TS msg);
}

// ReSharper disable once UnusedType.Global
public interface IWebsocket<T> : IWebsocket<T, T> { }