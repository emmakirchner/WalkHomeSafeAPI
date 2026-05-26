using Microsoft.EntityFrameworkCore;
using WalkHomeSafeAPI.Data;
using WalkHomeSafeAPI.Models.Context;
using WalkHomeSafeAPI.Models.Entities;

namespace WalkHomeSafeAPI.Services;

public class UserService(AppDbContext context) : IUserService
{
    public int GetUserIdFromDatabase(AppUserContext user)
    {
        var dbUser = context.Users.SingleOrDefault(u => u.FirebaseId == user.FirebaseId);
        if (dbUser is null)
        {
            var newUser = new UserEntity
            {
                FirebaseId = user.FirebaseId,
                UserName = user.UserName,
                IsActive = true
            };

            context.Users.Add(newUser);
            context.SaveChanges();

            return newUser.Id;

        }
        else if (dbUser.IsActive is false)
        {
            return 0;
        }
        else return dbUser.Id;
    }

    public bool DeleteUser(AppUserContext user)
    {
        var dbUser = context.Users.FirstOrDefault(u => u.FirebaseId == user.FirebaseId);

        if (dbUser == null)
            return false;

        dbUser.IsActive = false;
        context.SaveChanges();

        return true;
    }
}
