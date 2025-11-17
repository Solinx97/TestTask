using UserAPI.Data;
using UserAPI.Exceptions;
using UserAPI.Interfaces;
using UserAPI.Models;

namespace UserAPI.Services;

internal class NodeService(UserContext context) : INodeService
{
    private readonly UserContext _context = context;

    public async Task<NodeModel?> CreateAsync(string treeName, string nodeName, int? parentNodeId = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(treeName), treeName);
        ArgumentException.ThrowIfNullOrEmpty(nameof(nodeName), nodeName);

        NodeModel node;
        if (parentNodeId.HasValue)
        {
            await SecureException.ThrowIfParentNotExist(_context, parentNodeId.Value);
            await SecureException.ThrowIfNodeNameNotUnique(_context, nodeName, parentNodeId.Value);

            node = await CreateNewNodeAsync(parentNodeId.Value, nodeName);
        }
        else
        {
            node = await CreateNewTreeAsync(treeName);
        }

        return node;
    }

    public async Task DeleteAsync(int nodeId)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(nodeId, 1);

        await SecureException.ThrowIfAnyChildrenExist(_context, nodeId);

        var entity = await _context.Set<NodeModel>().FindAsync(nodeId);
        ArgumentNullException.ThrowIfNull(entity);

        _context.Set<NodeModel>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task RenameAsync(int nodeId, string newName)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(nodeId, 1);
        ArgumentException.ThrowIfNullOrEmpty(nameof(newName), newName);

        var node = await _context.Set<NodeModel>().FindAsync(nodeId);
        ArgumentNullException.ThrowIfNull(node, nameof(node));

        await SecureException.ThrowIfNodeNameNotUnique(_context, newName, node.ParentNodeId ?? 0);

        node.Name = newName;

        await _context.SaveChangesAsync();
    }

    private async Task<NodeModel> CreateNewTreeAsync(string treeName)
    {
        var newTree = new NodeModel
        {
            Name = treeName,
        };

        var entityEntry = await _context.Set<NodeModel>().AddAsync(newTree);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    private async Task<NodeModel> CreateNewNodeAsync(int parentNodeId, string nodeName)
    {
        var newNode = new NodeModel
        {
            Name = nodeName,
            ParentNodeId = parentNodeId,
        };

        var entityEntry = await _context.Set<NodeModel>().AddAsync(newNode);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }
}
