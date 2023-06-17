namespace ApplicationsApi.Repository;

using Models;
using MongoDB.Driver;

public static class Database {
    private static string ConnectionString =>
        "mongodb://applications:dd4d8392f929e5e77bf65cc15eb8547dafa2edeacd542b603c2d353536edc4a3597e4fff8bab64d2c783a107399746002f98a3b68eb6809e33885115f842e42a7a4dbad26423376cc76112f85b10c7c9@api.studyum.net:32/Applications";

    private static MongoClient Client => new(ConnectionString);

    private static IMongoDatabase Db =>
        Client.GetDatabase("Applications");

    public static IMongoCollection<ApplicationTemplate> TemplatesCollection =>
        Db.GetCollection<ApplicationTemplate>("Templates");
    
}