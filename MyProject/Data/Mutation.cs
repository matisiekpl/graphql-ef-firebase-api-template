using System.Security.Claims;
using MyProject.DTO;
using MyProject.Entities;
using MyProject.Services;

namespace MyProject.Data;

public class Mutation
{
    public async Task<User> UpdateUser(ClaimsPrincipal claimsPrincipal, UpdateUserInput input, IUserService userService)
        => await userService.UpdateUser(await userService.GetUser(claimsPrincipal), input);
}