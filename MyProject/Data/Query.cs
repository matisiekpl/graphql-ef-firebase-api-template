using System.Security.Claims;
using MyProject.Entities;
using MyProject.Services;

namespace MyProject.Data;

public class Query
{
    public async Task<User> Me(ClaimsPrincipal claimsPrincipal, IUserService userService) =>
        await userService.GetUser(claimsPrincipal);
}