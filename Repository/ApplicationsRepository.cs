using ApplicationsApi.Models;
using MongoDB.Driver;

namespace ApplicationsApi.Repository;

public static class ApplicationsRepository {
    public static IEnumerable<Application> List(string studyPlaceId, string? userId = null) {
        return Database.ApplicationsCollection
            .Find(t => t.StudyPlaceID == studyPlaceId && (userId == null || t.UserID == userId))
            .ToEnumerable();
    }

    public static void Create(Application application) {
        Database.ApplicationsCollection.InsertOne(application);
    }
}