using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ApplicationsApi.Utils;

public static class Json {
    public static T Deserialize<T>(string json) {
        if (typeof(T) == typeof(object)) return (T)ParseValue(JsonConvert.DeserializeObject<JObject>(json)!);
        return JsonConvert.DeserializeObject<T>(json)!;
    }

    public static object Deserialize(string json) => Deserialize<object>(json);

    public static string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj, Settings);

    public static object ParseValue(object obj) {
        return obj switch {
            JArray jArray => jArray.ToObject<object[]>()!.Select(ParseValue).ToArray(),
            JObject jObject => jObject.ToObject<Dictionary<string, object>>()!
                .ToDictionary(p => p.Key, p => ParseValue(p.Value)),
            JValue jValue => jValue.Value!,
            _ => obj
        };
    }

    private static readonly JsonSerializerSettings Settings = new() {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };
}