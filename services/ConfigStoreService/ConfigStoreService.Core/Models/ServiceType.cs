namespace ConfigStoreService.Core.Models;

public static class ServiceType
{
    public const string EmailMarketing = "EmailMarketing";
    public const string VideoConferencing = "VideoConferencing";
    
    public static readonly string[] SupportedServiceTypes = [EmailMarketing, VideoConferencing];
}