using UserAPI.Models;

namespace UserAPI.Interfaces;

public interface INodeService
{
    Task<NodeModel?> CreateAsync(string treeName, string nodeName, int? parentNodeId);

    Task DeleteAsync(int nodeId);

    Task RenameAsync(int nodeId, string newName);
}
