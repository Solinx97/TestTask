using UserAPI.Models;

namespace UserAPI.Interfaces;

public interface IUserService
{
    Task<UserModel?> CreateAsync(string code);

    Task<UserModel?> GetAsync(string code);
}
