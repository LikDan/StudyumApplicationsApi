using ApplicationsApi.Models;
using MongoDB.Driver;

namespace ApplicationsApi.Repository;

public static class TemplateRepository {
    public static ApplicationTemplate FindById(string id) {
        return Database
            .TemplatesCollection
            .Find(v => v.Id == id)
            .First();
    }
}