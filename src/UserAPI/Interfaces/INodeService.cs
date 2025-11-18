using UserAPI.Models;

namespace UserAPI.Interfaces;

/// <summary>
/// Contracts for creating, retrieving, updating and deleting Nodes
/// </summary>
public interface INodeService
{
    /// <summary>
    /// Create a new Node if provided nodeName and parentNodeId. Otherwise, create a new tree
    /// </summary>
    /// <param name="treeName">Name for a new tree. If not provided nodeName and parentNodeId, will create a new tree, use treeName</param>
    /// <param name="nodeName">Name for a new node. If not provided instead will create a new tree</param>
    /// <param name="parentNodeId">Parent node ID for a new node. If not provided instead will create a new tree</param>
    /// <returns>Created node or tree</returns>
    Task<NodeModel?> CreateAsync(string treeName, string? nodeName = null, int? parentNodeId = null);

    /// <summary>
    /// Get tree by provided tree name. If no any tree - will create a new tree
    /// </summary>
    /// <param name="treeName">Tree name that reflect a tree</param>
    /// <returns>Found tree or created tree</returns>
    Task<NodeModel?> GetByTreeName(string treeName);

    /// <summary>
    /// Delete node by Node ID. Before delete parent node should be deleted or childs. Otherwise, will throw exception
    /// </summary>
    /// <param name="nodeId">Node ID that reflect which node should be deleted</param>
    /// <returns>Task to catch any exceptions and await result</returns>
    Task DeleteAsync(int nodeId);

    /// <summary>
    /// Rename exist node by new name. Name should be unique by siblings. Otherwise, will throw exception
    /// </summary>
    /// <param name="nodeId">Node ID that reflect which node name should be renamed</param>
    /// <param name="newName">New name for node. New name should be unique by siblings</param>
    /// <returns>Task to catch any exceptions and await result</returns>
    Task RenameAsync(int nodeId, string newName);
}
