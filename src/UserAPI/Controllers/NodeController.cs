using Microsoft.AspNetCore.Mvc;
using UserAPI.Exceptions;
using UserAPI.Interfaces;

namespace UserAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NodeController(INodeService service, ILogger<NodeController> logger) : ControllerBase
{
    private readonly INodeService _service = service;
    private readonly ILogger<NodeController> _logger = logger;

    [HttpPost]
    public async Task<IActionResult> Create(string treeName, string nodeName, int? parentNodeId)
    {
        try
        {
            var node = await _service.CreateAsync(treeName, nodeName, parentNodeId);

            return Ok(node);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError("Some argument was null or empty: {Arg}", ex.ParamName);

            return BadRequest();
        }
        catch (SecureException ex)
        {
            _logger.LogError(ex.Message);

            return BadRequest();
        }
    }

    [HttpDelete("{nodeId:int:min(1)}")]
    public async Task<IActionResult> Delete(int nodeId)
    {
        try
        {
            await _service.DeleteAsync(nodeId);

            return NoContent();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError("Some argument was incorrect: {Arg}", ex.ParamName);

            return BadRequest();
        }
        catch (SecureException ex)
        {
            _logger.LogError(ex.Message);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError("Some argument was null: {Arg}", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpPatch("{nodeId:int:min(1)}")]
    public async Task<IActionResult> Rename(int nodeId, string newNodeName)
    {
        try
        {
            await _service.RenameAsync(nodeId, newNodeName);

            return NoContent();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError("Some argument was incorrect: {Arg}", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError("Some argument was null: {Arg}", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentException ex)
        {
            _logger.LogError("Some argument was null or empty: {Arg}", ex.ParamName);

            return BadRequest();
        }
        catch (SecureException ex)
        {
            _logger.LogError(ex.Message);

            return BadRequest();
        }
    }
}
