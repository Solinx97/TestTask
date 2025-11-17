using Microsoft.EntityFrameworkCore;
using UserAPI.Data;
using UserAPI.Models;

namespace UserAPI.Exceptions;

public class SecureException(string message) : Exception(message)
{
    public string Message { get; } = message;

    public static async Task ThrowIfAnyChildrenExist(UserContext context, int nodeId)
    {
        var count = await context.Set<NodeModel>()
            .AsNoTracking()
            .CountAsync(n => n.ParentNodeId == nodeId);
        if (count > 0)
        {
            throw new SecureException("You have to delete all children nodes first");
        }
    }

    public static async Task ThrowIfParentNotExist(UserContext context, int parentId)
    {
        var count = await context.Set<NodeModel>()
            .AsNoTracking()
            .CountAsync(n => n.Id == parentId);
        if (count == 0)
        {
            throw new SecureException("The parent with the specified ID does not exist");
        }
    }

    public static async Task ThrowIfNodeNameNotUnique(UserContext context, string nodeName, int parentId)
    {
        var count = await context.Set<NodeModel>()
            .AsNoTracking()
            .CountAsync(n => n.ParentNodeId == parentId && n.Name == nodeName);
        if (count > 0)
        {
            throw new SecureException("Node name should be unique across all siblings");
        }
    }

    public static async Task ThrowIfUserExist(UserContext context, string code)
    {
        var count = await context.Set<UserModel>()
            .AsNoTracking()
            .CountAsync(n => n.Code == code);
        if (count > 0)
        {
            throw new SecureException("User with this code already exist");
        }
    }
}
