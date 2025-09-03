using VideoConferencingService.Core.Models;

namespace VideoConferencingService.Core.Ports.Outgoing;

public interface IVideoConferencingRepository
{
    Task<VideoConferencing> CreateAsync(VideoConferencing configuration);
    Task<VideoConferencing?> GetByAccountIdAsync(string accountId);
    Task<VideoConferencing?> GetByIdAsync(string id);
    Task<VideoConferencing?> UpdateAsync(VideoConferencing configuration);
    Task<bool> DeleteByAccountIdAsync(string accountId);
    Task<IQueryable<VideoConferencing>> GetAllAsync();
}