using ApplicationsApi.Controllers;
using ApplicationsApi.Proto;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationsApi.Middlewares; 

public static class AuthMiddleware {
    public static User Auth(this ControllerBase c, params string[] permissions) {
        var tokens = c.Request.Headers["Authorization"].FirstOrDefault()?.Split(":");
        if (tokens is not { Length: 2 }) throw new ArgumentException("Bad claims");
        return UserController.User(tokens[0], tokens[1], permissions);
    }
}