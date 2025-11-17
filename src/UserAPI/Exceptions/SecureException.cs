using Microsoft.EntityFrameworkCore;
using UserAPI.Data;
using UserAPI.Models;

namespace UserAPI.Exceptions;

public class SecureException(string message) : Exception(message)
{
    public string Message { get; } = message;

    public static async Task ThrowIfAnyChildrenExist(UserContext context, int nodeId)
    {
        var countChildrens = await context.Set<NodeModel>()
            .AsNoTracking()
            .CountAsync(n => n.ParentNodeId == nodeId);
        if (countChildrens > 0)
        {
            throw new SecureException("You have to delete all children nodes first");
        }
    }

    public static async Task ThrowIfParentNotExist(UserContext context, int parentId)
    {
        var countChildrens = await context.Set<NodeModel>()
            .AsNoTracking()
            .CountAsync(n => n.Id == parentId);
        if (countChildrens == 0)
        {
            throw new SecureException("The parent with the specified ID does not exist");
        }
    }

    public static async Task ThrowIfNodeNameNotUnique(UserContext context, string nodeName, int parentId)
    {
        var countChildrens = await context.Set<NodeModel>()
            .AsNoTracking()
            .CountAsync(n => n.ParentNodeId == parentId && n.Name == nodeName);
        if (countChildrens > 0)
        {
            throw new SecureException("Node name should be unique across all siblings");
        }
    }
}
