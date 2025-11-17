using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserAPI.Enums;
using UserAPI.Exceptions;
using UserAPI.Interfaces;

namespace UserAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TreeController(INodeService nodeService, IJournalService journalService, ILogger<TreeController> logger) : ControllerBase
{
    private readonly INodeService _nodeService = nodeService;
    private readonly IJournalService _journalService = journalService;
    private readonly ILogger<TreeController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetByTreeName([Required] string treeName)
    {
        try
        {
            var tree = await _nodeService.GetByTreeName(treeName);

            return Ok(tree);
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
