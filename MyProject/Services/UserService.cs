using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MyProject.Database;
using MyProject.DTO;
using MyProject.Entities;
using MyProject.Exceptions;

namespace MyProject.Services;

public class UserService : IUserService
{
    private readonly DatabaseContext _databaseContext;

    public UserService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<User> GetUser(ClaimsPrincipal claimsPrincipal)
    {
        var firebaseUserId = GetFirebaseUserId(claimsPrincipal);
        if (firebaseUserId == null)
            throw new ForbiddenException();
        User? user;
        if (await _databaseContext.Users.AnyAsync(x => x.FirebaseUserId == firebaseUserId))
        {
            user = await _databaseContext.Users
                .Where(x => x.FirebaseUserId == firebaseUserId)
                .FirstAsync();
            if (user.Email == claimsPrincipal.Claims
                    .First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                    .Value) return user;
            user.Email = claimsPrincipal.Claims
                .First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync();
            return user;
        }

        user = new User(firebaseUserId,
            claimsPrincipal.Claims
                .First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value);
        await _databaseContext.Users.AddAsync(user);
        await _databaseContext.SaveChangesAsync();
        user = await _databaseContext.Users
            .Where(x => x.FirebaseUserId == firebaseUserId)
            .FirstAsync();
        return user;
    }

    public async Task<User> FindUser(Guid id)
    {
        return await _databaseContext.Users.FirstAsync(x => x.UserId == id);
    }

    public async Task<User> UpdateUser(User user, UpdateUserInput input)
    {
        if (input.FirstName != null) user.FirstName = input.FirstName;
        if (input.LastName != null) user.LastName = input.LastName;
        _databaseContext.Users.Update(user);
        await _databaseContext.SaveChangesAsync();
        return user;
    }

    private static string? GetFirebaseUserId(ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.HasClaim(x => x.Type == "user_id")
            ? claimsPrincipal.Claims.First(x => x.Type == "user_id").Value
            : null;
    }
}