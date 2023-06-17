namespace ApplicationsApi.Utils.Converters.Serializers;

public class JsonByteSerializer<T> : ByteSerializer<T> {
    private readonly IConverter<byte[], T> _serializer = SimpleConverters.BytesToJson<T>();
    private readonly IConverter<T, byte[]> _deserializer = SimpleConverters.JsonToBytes<T>();

    public override IConverter<byte[], T> SerializeConverter() => _serializer;
    public override IConverter<T, byte[]> DeserializeConverter() => _deserializer;
}
