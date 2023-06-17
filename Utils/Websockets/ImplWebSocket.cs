using System.Net.WebSockets;
using ApplicationsApi.Utils.Converters;

namespace ApplicationsApi.Utils.Websockets;

public abstract class ImplWebSockets<TR, TS> : IWebsocket<TR, TS> {
    public long BufferSize => 1024 * 4;
    public abstract WebSocket WebSocket { get; }

    public Action<TR>? OnReceive { get; set; }

    protected abstract IConverter<byte[], TR> ReceiverConverter { get; }
    protected abstract IConverter<TS, byte[]> SenderConverter { get; }

    public void StartReceiving() {
        using (WebSocket) {
            var buffer = new byte[BufferSize];
            var arraySegment = new ArraySegment<byte>(buffer);
            WebSocketReceiveResult result;
            do {
                result = WebSocket.ReceiveAsync(arraySegment, CancellationToken.None).Result;
                if (result.Count == 0) continue;
                var receivedValue = ReceiverConverter.Convert(arraySegment[..result.Count].ToArray());
                OnReceive?.Invoke(receivedValue);
            } while (!result.CloseStatus.HasValue);
        }
    }

    public void Send(TS msg) {
        var bytes = SenderConverter.Convert(msg);
        var memory = new ReadOnlyMemory<byte>(bytes);
        WebSocket.SendAsync(memory, WebSocketMessageType.Text, true, CancellationToken.None).Preserve();
    }
}

public abstract class ImplWebSockets<T> : ImplWebSockets<T, T> { }