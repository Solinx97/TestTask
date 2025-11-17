using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserAPI.Enums;
using UserAPI.Exceptions;
using UserAPI.Interfaces;

namespace UserAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NodeController(INodeService nodeService, IJournalService journalService , ILogger<NodeController> logger) : ControllerBase
{
    private readonly INodeService _nodeService = nodeService;
    private readonly IJournalService _journalService = journalService;
    private readonly ILogger<NodeController> _logger = logger;

    [HttpPost]
    public async Task<IActionResult> Create([Required] string treeName, string? nodeName, int? parentNodeId)
    {
        try
        {
            var node = await _nodeService.CreateAsync(treeName, nodeName, parentNodeId);

            return Ok(node);
        }
        catch (SecureException ex)
        {
            _logger.LogError(ex.Message);

            var journal = await _journalService.CreateAsync(ExceptionType.Secure, ex.Message);

            var errorResponse = new
            {
                id = journal == null ? DateTime.UtcNow.Ticks.ToString() : journal.Id.ToString(),
                type = ExceptionType.Secure,
                data = new
                {
                    message = ex.Message
                }
            };

            return StatusCode(500, errorResponse);
        }
    }

    [HttpDelete("{nodeId:int:min(1)}")]
    public async Task<IActionResult> Delete(int nodeId)
    {
        try
        {
            await _nodeService.DeleteAsync(nodeId);

            return NoContent();
        }
        catch (SecureException ex)
        {
            _logger.LogError(ex.Message);

            var journal = await _journalService.CreateAsync(ExceptionType.Secure, ex.Message);

            var errorResponse = new
            {
                id = journal == null ? DateTime.UtcNow.Ticks.ToString() : journal.Id.ToString(),
                type = ExceptionType.Secure,
                data = new
                {
                    message = ex.Message
                }
            };

            return StatusCode(500, errorResponse);
        }
    }

    [HttpPatch("{nodeId:int:min(1)}")]
    public async Task<IActionResult> Rename(int nodeId, [Required] string newNodeName)
    {
        try
        {
            await _nodeService.RenameAsync(nodeId, newNodeName);

            return NoContent();
        }
        catch (SecureException ex)
        {
            _logger.LogError(ex.Message);

            var journal = await _journalService.CreateAsync(ExceptionType.Secure, ex.Message);

            var errorResponse = new
            {
                id = journal == null ? DateTime.UtcNow.Ticks.ToString() : journal.Id.ToString(),
                type = ExceptionType.Secure,
                data = new
                {
                    message = ex.Message
                }
            };

            return StatusCode(500, errorResponse);
        }
    }
}
