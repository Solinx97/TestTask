using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAPI.Data;
using UserAPI.Models;

namespace UserAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JournalController(UserContext context) : ControllerBase
{
    private readonly UserContext _context = context;

    [HttpGet]
    public async Task<IActionResult> GetRange(int skip, int take)
    {
        var journals = await _context.Set<JournalInfoModel>()
            .Skip(skip)
            .Take(take)
            .AsNoTracking()
            .ToArrayAsync();

        var journalRange = new JournalInfoRangeModel
        {
            Skip = skip,
            Count = journals.Count(),
            Items = journals
        };

        return Ok(journalRange);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetSingle(int id)
    {
        var journal = await _context.Set<JournalInfoModel>()
            .FindAsync(id);

        return Ok(journal);
    }
}
