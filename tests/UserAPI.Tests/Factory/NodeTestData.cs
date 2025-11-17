using UserAPI.Models;

namespace UserAPI.Tests.Factory;

internal static class NodeTestData
{
    public static List<NodeModel> CreateTree(int descendants = 0, int childrens = 0, string treeName = "Tree")
    {
        int count = 1;
        var tree = new List<NodeModel>
        {
            new() {
                Id = count,
                Name = treeName,
            }
        };

        for (var i = 0; i < descendants; i++)
        {
            SelectParent(tree, i, (int)Math.Pow(childrens, i), childrens, ref count, count - 1);
        }

        return tree;
    }

    private static void SelectParent(List<NodeModel> tree, int currentDescendant, int parents, int childrens, ref int count, int existElements)
    {
        for (var i = 0; i < parents && childrens > 0; i++)
        {
            AddChildrens(tree, currentDescendant, childrens, tree[existElements - i].Id, ref count);
        }
    }

    private static void AddChildrens(List<NodeModel> tree, int currentDescendant, int childrens, int parentId, ref int count)
    {
        for (var i = 0; i < childrens; i++)
        {
            var children = new NodeModel
            {
                Id = ++count,
                Name = $"Node-{currentDescendant}-{i}",
                ParentNodeId = parentId
            };
            tree.Add(children);
        }
    }
}
