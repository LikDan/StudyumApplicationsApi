using ApplicationsApi.Proto;
namespace ApplicationsApi.Controllers;

using Grpc.Net.Client;

public class UserController {
    public static User User(string access, string refresh, params string[] permissions) {
        using var channel = GrpcChannel.ForAddress("api:4772");
        var client = new Auth.AuthClient(channel);
        var jwt = new JWT {
            Access = access,
            Refresh = refresh,
        };
        var response = client.AuthUser(new AuthRequest {
            Jwt = jwt,
            RequiredPermissions = { permissions }
        });
        return response.User;
    }
}