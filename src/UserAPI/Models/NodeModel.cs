namespace UserAPI.Models;

public class NodeModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? ParentNodeId { get; set; }
}
