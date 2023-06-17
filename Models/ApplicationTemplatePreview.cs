using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationsApi.Models; 

public class ApplicationTemplatePreview {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("title")]
    public string Title { get; set; } = "";
}
