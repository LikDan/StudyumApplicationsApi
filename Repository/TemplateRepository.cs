using ApplicationsApi.Models;
using MongoDB.Driver;

namespace ApplicationsApi.Repository;

public static class TemplateRepository {
    public static ApplicationTemplate? FindById(string id, string studyPlaceId) {
        return Database
            .TemplatesCollection
            .Find(v => v.Id == id && v.StudyPlaceID == studyPlaceId)
            .FirstOrDefault();
    }

    public static void Create(ApplicationTemplate template) {
        Database.TemplatesCollection.InsertOne(template);
    }

    public static IEnumerable<ApplicationTemplatePreview> PreviewList(string studyPlaceId) {
        return Database.TemplatesCollection
            .Find(t => t.StudyPlaceID == studyPlaceId)
            .ToEnumerable()
            .Select(v => v.Preview());
    }

    public static void Delete(string id) {
        Database.TemplatesCollection.DeleteOne(v => v.Id == id);
    }
}