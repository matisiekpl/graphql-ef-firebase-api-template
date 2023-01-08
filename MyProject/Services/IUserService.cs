using System.Security.Claims;
using MyProject.DTO;
using MyProject.Entities;

namespace MyProject.Services;

public interface IUserService
{
    public Task<User> GetUser(ClaimsPrincipal claimsPrincipal);
    public Task<User> FindUser(Guid id);
    public Task<User> UpdateUser(User user, UpdateUserInput input);
}