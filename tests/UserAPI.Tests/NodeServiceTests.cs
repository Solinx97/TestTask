using Microsoft.EntityFrameworkCore;
using UserAPI.Data;
using UserAPI.Exceptions;
using UserAPI.Models;
using UserAPI.Services;
using UserAPI.Tests.Factory;

namespace UserAPI.Tests;

public class NodeServiceTests : ServiceTestsBase
{
    [Fact]
    public async Task CreateAsync_Entity_ShouldCreateAndReturnCreatedEntity()
    {
        // Arrange
        const int descendants = 3;
        const int childrens = 3;
        const int nodeId = 41;
        const int parentId = 40;

        using var context = CreateInMemoryContext(nameof(CreateAsync_Entity_ShouldCreateAndReturnCreatedEntity));
        await context.Set<NodeModel>().AddRangeAsync(NodeTestData.CreateTree(descendants, childrens));
        await context.SaveChangesAsync();

        var service = new NodeService(context);

        // Act
        var result = await service.CreateAsync("tree 1", "node", parentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(nodeId, result.Id);
        Assert.Equal(41, context.Set<NodeModel>().Count());
    }

    [Fact]
    public async Task CreateAsync_ThrowSecureException_ShouldNotCreateAndThrowSecureExceptionAsParentNotExist()
    {
        // Arrange
        const int descendants = 3;
        const int childrens = 3;
        const int parentId = 70;

        using var context = CreateInMemoryContext(nameof(CreateAsync_ThrowSecureException_ShouldNotCreateAndThrowSecureExceptionAsParentNotExist));
        await context.Set<NodeModel>().AddRangeAsync(NodeTestData.CreateTree(descendants, childrens));
        await context.SaveChangesAsync();

        var service = new NodeService(context);

        // Act and Assert
        await Assert.ThrowsAsync<SecureException>(() => service.CreateAsync("tree 1", "node", parentId));
    }

    [Fact]
    public async Task CreateAsync_ThrowSecureException_ShouldNotCreateAndThrowSecureExceptionThatNameShouldBeUniqueBySiblings()
    {
        // Arrange
        const int descendants = 3;
        const int childrens = 3;
        const int parentId = 5;

        using var context = CreateInMemoryContext(nameof(CreateAsync_ThrowSecureException_ShouldNotCreateAndThrowSecureExceptionThatNameShouldBeUniqueBySiblings));
        await context.Set<NodeModel>().AddRangeAsync(NodeTestData.CreateTree(descendants, childrens));
        await context.SaveChangesAsync();

        var service = new NodeService(context);

        // Act and Assert
        await Assert.ThrowsAsync<SecureException>(() => service.CreateAsync("tree 1", "Node-2-2", parentId));
    }

    [Fact]
    public async Task GetByTreeName_Entity_ShouldReturnExistTree()
    {
        // Arrange
        const int descendants = 3;
        const int childrens = 3;
        const int treeId = 1;
        const string treeName = "Tree";

        using var context = CreateInMemoryContext(nameof(GetByTreeName_Entity_ShouldReturnExistTree));
        await context.Set<NodeModel>().AddRangeAsync(NodeTestData.CreateTree(descendants, childrens));
        await context.SaveChangesAsync();

        var service = new NodeService(context);

        // Act
        var result = await service.GetByTreeName(treeName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(treeId, result.Id);
        Assert.Equal(treeName, result.Name);
    }

    [Fact]
    public async Task GetByTreeName_Entity_ShouldCreateNewTreeAndReturnCreatedTree()
    {
        // Arrange
        const int descendants = 3;
        const int childrens = 3;
        const int treeId = 41;
        const string treeName = "Tree-1";

        using var context = CreateInMemoryContext(nameof(CreateAsync_Entity_ShouldCreateAndReturnCreatedEntity));
        await context.Set<NodeModel>().AddRangeAsync(NodeTestData.CreateTree(descendants, childrens));
        await context.SaveChangesAsync();

        var service = new NodeService(context);

        // Act
        var result = await service.GetByTreeName(treeName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(treeId, result.Id);
        Assert.Equal(treeName, result.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteAllChildrensAndEntity()
    {
        // Arrange
        const int descendants = 3;
        const int childrens = 3;
        const int deleteNodeId = 2;

        using var context = CreateInMemoryContext(nameof(DeleteAsync_ShouldDeleteAllChildrensAndEntity));
        await context.Set<NodeModel>().AddRangeAsync(NodeTestData.CreateTree(descendants, childrens));
        await context.SaveChangesAsync();

        var service = new NodeService(context);

        // Act
        var toRemove = new List<NodeModel>();
        var getChildrens = await GetAllChildrens(context, toRemove, deleteNodeId);

        for (var i = getChildrens.Count - 1; i >= 0; i--)
        {
            await service.DeleteAsync(getChildrens[i].Id);
        }

        await service.DeleteAsync(deleteNodeId);

        // Assert
        Assert.Equal(27, context.Set<NodeModel>().Count());
    }

    [Fact]
    public async Task DeleteAsync_ThrowSecureException_ShouldNotDeleteAndThrowSecureExceptionThatShouldBeRemoveChildsFirst()
    {
        // Arrange
        const int descendants = 3;
        const int childrens = 3;
        const int deleteNodeId = 2;

        using var context = CreateInMemoryContext(nameof(DeleteAsync_ThrowSecureException_ShouldNotDeleteAndThrowSecureExceptionThatShouldBeRemoveChildsFirst));
        await context.Set<NodeModel>().AddRangeAsync(NodeTestData.CreateTree(descendants, childrens));
        await context.SaveChangesAsync();

        var service = new NodeService(context);

        // Act and Assert
        await Assert.ThrowsAsync<SecureException>(() => service.DeleteAsync(deleteNodeId));
    }

    [Fact]
    public async Task RenameAsync_ShouldRenameNodeName()
    {
        // Arrange
        const int descendants = 3;
        const int childrens = 3;
        const int renameNodeId = 35;
        const string newName = "Check again";

        using var context = CreateInMemoryContext(nameof(RenameAsync_ShouldRenameNodeName));
        await context.Set<NodeModel>().AddRangeAsync(NodeTestData.CreateTree(descendants, childrens));
        await context.SaveChangesAsync();

        var service = new NodeService(context);

        // Act
        await service.RenameAsync(renameNodeId, newName);
        var renamedNode = await context.Set<NodeModel>().FindAsync(renameNodeId);

        // Assert
        Assert.NotNull(renamedNode);
        Assert.Equal(newName, renamedNode.Name);
    }

    [Fact]
    public async Task RenameAsync_ShouldRenameNodeNameWhenUseSameNameAsChildrens()
    {
        // Arrange
        const int descendants = 3;
        const int childrens = 3;
        const int renameNodeId = 10;
        const string newName = "Node-2-2";

        using var context = CreateInMemoryContext(nameof(RenameAsync_ShouldRenameNodeNameWhenUseSameNameAsChildrens));
        await context.Set<NodeModel>().AddRangeAsync(NodeTestData.CreateTree(descendants, childrens));
        await context.SaveChangesAsync();

        var service = new NodeService(context);

        // Act
        await service.RenameAsync(renameNodeId, newName);
        var renamedNode = await context.Set<NodeModel>().FindAsync(renameNodeId);

        // Assert
        Assert.NotNull(renamedNode);
        Assert.Equal(newName, renamedNode.Name);
    }

    [Fact]
    public async Task RenameAsync_ThrowSecureException_ShouldNotRenameNodeNameAndThrowSecureExceptionThatNameShouldBeUniqueBySiblings()
    {
        // Arrange
        const int descendants = 3;
        const int childrens = 3;
        const int renameNodeId = 30;
        const string newName = "Node-2-2";

        using var context = CreateInMemoryContext(nameof(RenameAsync_ThrowSecureException_ShouldNotRenameNodeNameAndThrowSecureExceptionThatNameShouldBeUniqueBySiblings));
        await context.Set<NodeModel>().AddRangeAsync(NodeTestData.CreateTree(descendants, childrens));
        await context.SaveChangesAsync();

        var service = new NodeService(context);

        // Act and Assert
        await Assert.ThrowsAsync<SecureException>(() => service.RenameAsync(renameNodeId, newName));
    }

    private static async Task<List<NodeModel>> GetAllChildrens(UserContext context, List<NodeModel> childrens, int deleteNodeId)
    {
        var getChildrens = await context.Set<NodeModel>()
            .Where(x => x.ParentNodeId == deleteNodeId)
            .AsNoTracking()
            .ToListAsync();

        childrens.AddRange(getChildrens);

        if (getChildrens.Count > 0)
        {
            foreach (var child in getChildrens)
            {
                await GetAllChildrens(context, childrens, child.Id);
            }
        }

        return childrens;
    }
}
