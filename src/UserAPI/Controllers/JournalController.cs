using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserAPI.Interfaces;

namespace UserAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class JournalController(IJournalService service) : ControllerBase
{
    private readonly IJournalService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetRange([Range(0, int.MaxValue)] int skip, [Range(0, int.MaxValue)] int take)
    {
        var journalRange = await _service.GetRangeAsync(skip, take);

        return Ok(journalRange);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetSingle(int id)
    {
        var journal = await _service.GetSingleAsync(id);

        return Ok(journal);
    }
}
