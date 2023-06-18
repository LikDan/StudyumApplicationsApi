using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationsApi.Models;

public class CdnEntry {
    [BsonElement("originFileName")] public string OriginFileName { get; set; } = "";
    [BsonElement("fileName")] public string FileName { get; set; } = "";
    [BsonElement("shortenFileName")] public string ShortenFileName { get; set; } = "";
}