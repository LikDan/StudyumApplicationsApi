using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationsApi.Models; 

public class ApplicationTemplate {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("title")]
    public string Title { get; set; } = "";
    [BsonElement("scheme")]
    public InputScheme[] Scheme { get; set; }
    [BsonElement("template")]
    public string Template { get; set; } = "";
    [BsonElement("timeout")]
    public int Timeout { get; set; } = -1;
    public string StudyPlaceID { get; set; } = "";

    public ApplicationTemplatePreview Preview() => new() { Id = Id, Title = Title };
}

public class InputScheme {
    [BsonElement("parameter")]
    public string Parameter { get; set; } = "";
    [BsonElement("prompt")]
    public string Prompt { get; set; } = "";
    [BsonElement("title")]
    public string Title { get; set; } = "";
    [BsonElement("type")]
    public string Type { get; set; } = "";
    [BsonElement("validators")]
    public Validator[] Validators { get; set; }
}

public class Validator {
    [BsonElement("name")]
    public string Name { get; set; } = "";
    [BsonElement("notValidMessage")]
    public string NotValidMessage { get; set; } = "";
    [BsonElement("value")]
    public object Value { get; set; } = "";
}
