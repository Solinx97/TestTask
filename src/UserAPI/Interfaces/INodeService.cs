using UserAPI.Models;

namespace UserAPI.Interfaces;

public interface INodeService
{
    Task<NodeModel?> CreateAsync(string treeName, string? nodeName = null, int? parentNodeId = null);

    Task<NodeModel?> GetByTreeName(string treeName);

    Task DeleteAsync(int nodeId);

    Task RenameAsync(int nodeId, string newName);
}
