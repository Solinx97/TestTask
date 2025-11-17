using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAPI.Data;
using UserAPI.Models;

namespace UserAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TreeController(UserContext context) : ControllerBase
{
    private readonly UserContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Get(string treeName)
    {
        var tree = await _context.Set<NodeModel>()
            .FirstOrDefaultAsync(t => string.Equals(t.Name, treeName, StringComparison.OrdinalIgnoreCase)) ?? await CreateTreeAsync(treeName);
        return Ok(tree);
    }

    private async Task<NodeModel> CreateTreeAsync(string treeName)
    {
        var newTree = new NodeModel
        {
            Name = treeName
        };

        var entityEntry = await _context.Set<NodeModel>().AddAsync(newTree);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }
}
