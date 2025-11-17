using Microsoft.EntityFrameworkCore;
using UserAPI.Data;
using UserAPI.Enums;
using UserAPI.Interfaces;
using UserAPI.Models;

namespace UserAPI.Services;

internal class JournalService(UserContext context) : IJournalService
{
    private readonly UserContext _context = context;

    public async Task<JournalModel?> CreateAsync(ExceptionType type, string data)
    {
        var journal = new JournalModel
        {
            Type = type,
            Data = data,
            CreateAt = DateTime.UtcNow,
        };

        var entityEntry = await _context.Set<JournalModel>().AddAsync(journal);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<JournalRangeModel> GetRangeAsync(int skip, int take)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(skip);
        ArgumentOutOfRangeException.ThrowIfNegative(take);

        var journals = await _context.Set<JournalModel>()
            .Skip(skip)
            .Take(take)
            .AsNoTracking()
            .ToArrayAsync();

        var journalRange = new JournalRangeModel
        {
            Skip = skip,
            Count = journals.Count(),
            Items = journals
        };

        return journalRange;
    }

    public async Task<JournalModel?> GetSingleAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var journal = await _context.Set<JournalModel>()
            .FindAsync(id);

        return journal;
    }
}
