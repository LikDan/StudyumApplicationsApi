using ApplicationsApi.Proto;

namespace ApplicationsApi.Controllers;

using Grpc.Net.Client;

public class StudyPlacesController {
    public static StudyPlace StudyPlace(string id) {
        using var channel = GrpcChannel.ForAddress("http://api.studyum.net:4772");
        var client = new StudyPlaces.StudyPlacesClient(channel);
        return client.GetByID(new IdRequest {Id = id})!;
    }
}