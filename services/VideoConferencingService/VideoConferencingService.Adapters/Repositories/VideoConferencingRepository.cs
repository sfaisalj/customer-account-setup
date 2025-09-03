using Microsoft.EntityFrameworkCore;
using VideoConferencingService.Core.Models;
using VideoConferencingService.Core.Ports.Outgoing;
using VideoConferencingService.Adapters.Data;

namespace VideoConferencingService.Adapters.Repositories;

public class VideoConferencingRepository : IVideoConferencingRepository
{
    private readonly VideoConferencingDbContext _context;

    public VideoConferencingRepository(VideoConferencingDbContext context)
    {
        _context = context;
    }

    public async Task<VideoConferencing> CreateAsync(VideoConferencing configuration)
    {
        _context.VideoConferencingConfigurations.Add(configuration);
        await _context.SaveChangesAsync();
        return configuration;
    }

    public async Task<VideoConferencing?> GetByAccountIdAsync(string accountId)
    {
        return await _context.VideoConferencingConfigurations
            .FirstOrDefaultAsync(v => v.AccountId == accountId);
    }

    public async Task<VideoConferencing?> GetByIdAsync(string id)
    {
        return await _context.VideoConferencingConfigurations
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<VideoConferencing?> UpdateAsync(VideoConferencing configuration)
    {
        _context.VideoConferencingConfigurations.Update(configuration);
        await _context.SaveChangesAsync();
        return configuration;
    }

    public async Task<bool> DeleteByAccountIdAsync(string accountId)
    {
        var configuration = await _context.VideoConferencingConfigurations
            .FirstOrDefaultAsync(v => v.AccountId == accountId);
            
        if (configuration == null)
            return false;

        _context.VideoConferencingConfigurations.Remove(configuration);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IQueryable<VideoConferencing>> GetAllAsync()
    {
        return _context.VideoConferencingConfigurations.AsQueryable();
    }
}