using WalkHomeSafeAPI.Models.Context;

namespace WalkHomeSafeAPI.Services;

public interface IUserService
{
    int GetUserIdFromDatabase(AppUserContext user);

    bool DeleteUser(AppUserContext user);
}
