using Microsoft.EntityFrameworkCore;
using UserAPI.Data;
using UserAPI.Exceptions;
using UserAPI.Interfaces;
using UserAPI.Models;

namespace UserAPI.Services;

internal class UserService(UserContext context) : IUserService
{
    private readonly UserContext _context = context;

    public async Task<UserModel?> CreateAsync(string code)
    {
        await SecureException.ThrowIfUserExist(_context, code);

        var user = new UserModel
        {
            Id = Guid.NewGuid().ToString(),
            Code = code
        };

        var entityEntry = await _context.Set<UserModel>().AddAsync(user);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<UserModel> GetAsync(string code)
    {
        ArgumentException.ThrowIfNullOrEmpty(code);

        var entity = await _context.Set<UserModel>().FirstOrDefaultAsync(u => u.Code == code);
        await _context.SaveChangesAsync();

        entity ??= await CreateAsync(code);

        return entity!;
    }
}
