using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationsApi.Models;

public class Application {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("studyPlaceID")] public string StudyPlaceID { get; set; } = "";
    [BsonElement("userID")] public string UserID { get; set; } = "";
    [BsonElement("templateID")] public string TemplateID { get; set; } = "";
    [BsonElement("data")] public Dictionary<string, object> Data { get; set; }
    [BsonElement("html")] public string Html { get; set; } = "";
    [BsonElement("cdn")] public CdnEntry CdnEntry { get; set; }
}