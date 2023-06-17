namespace ApplicationsApi.Utils.Converters.Serializers;

public class StringByteSerializer : ByteSerializer<string> {
    public override IConverter<byte[], string> SerializeConverter() => SimpleConverters.BytesToString;
    public override IConverter<string, byte[]> DeserializeConverter() => SimpleConverters.StringToBytes;
}