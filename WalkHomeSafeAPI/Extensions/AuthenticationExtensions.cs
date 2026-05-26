using System.Security.Claims;
using WalkHomeSafeAPI.Models.Context;

namespace WalkHomeSafeAPI.Extensions;

public static class AuthenticationExtensions
{
    public static AppUserContext? GetUserInfo(this ClaimsPrincipal user)
    {
        if (user.Identity?.IsAuthenticated != true)
            return null;

        var firebaseId = user.FindFirstValue("sub");

        if (string.IsNullOrEmpty(firebaseId))
            return null;

        return new AppUserContext
        {
            FirebaseId = firebaseId,
            UserName = user.FindFirstValue("name") ?? "User"
        };
    }
}
