using ApplicationsApi.Utils.Converters;
using ApplicationsApi.Utils.Converters.Serializers;

namespace ApplicationsApi.Utils.Websockets;

public class HttpWebSocket<TR, TS> : ImplHttpWebSockets<TR, TS> {
    protected override IConverter<byte[], TR> ReceiverConverter { get; }
    protected override IConverter<TS, byte[]> SenderConverter { get; }

    // ReSharper disable once MemberCanBeProtected.Global
    public HttpWebSocket(HttpContext context, IConverter<byte[], TR> receiverConverter,
        IConverter<TS, byte[]> senderConverter) : base(context) {
        ReceiverConverter = receiverConverter;
        SenderConverter = senderConverter;
    }
}

public class HttpWebSocket<T> : HttpWebSocket<T, T> {
    // ReSharper disable once UnusedMember.Global
    public HttpWebSocket(HttpContext context, IConverter<byte[], T> receiverConverter,
        IConverter<T, byte[]> senderConverter) : base(context, receiverConverter, senderConverter) { }

    public HttpWebSocket(HttpContext context, ImplSerializer<byte[], T> serializer) : base(context,
        serializer.SerializeConverter(), serializer.DeserializeConverter()) { }
}