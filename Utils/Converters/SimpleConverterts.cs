using System.Text;

namespace ApplicationsApi.Utils.Converters;

public static class SimpleConverters {
    public static readonly IConverter<byte[], string> BytesToString =
        new SimpleConverter<byte[], string>(Encoding.Default.GetString);

    public static readonly IConverter<string, byte[]> StringToBytes =
        new SimpleConverter<string, byte[]>(Encoding.UTF8.GetBytes);

    public static IConverter<string, T> StringToJson<T>() =>
        new SimpleConverter<string, T>(Json.Deserialize<T>);

    public static IConverter<T, string> JsonToString<T>() =>
        new SimpleConverter<T, string>(Json.Serialize);

    public static IConverter<byte[], T> BytesToJson<T>() =>
        new SimpleConverter<byte[], T>(v => StringToJson<T>().Convert(BytesToString.Convert(v)));

    public static IConverter<T, byte[]> JsonToBytes<T>() =>
        new SimpleConverter<T, byte[]>(v => StringToBytes.Convert(JsonToString<T>().Convert(v)));
}