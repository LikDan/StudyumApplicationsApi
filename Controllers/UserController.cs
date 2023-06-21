using ApplicationsApi.Proto;
namespace ApplicationsApi.Controllers;

using Grpc.Net.Client;

public class UserController {
    public static User User(string access, string refresh, params string[] permissions) {
        using var channel = GrpcChannel.ForAddress("http://api.studyum.net:4772");
        var client = new Auth.AuthClient(channel);
        var response = client.AuthUser(new AuthRequest {
            Jwt = new JWT {
                Access = access,
                Refresh = refresh,
            },
            RequiredPermissions = { permissions }
        });
        return response.User;
    }
}