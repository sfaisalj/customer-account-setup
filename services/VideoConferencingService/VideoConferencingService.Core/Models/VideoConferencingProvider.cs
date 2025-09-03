namespace VideoConferencingService.Core.Models;

public static class VideoConferencingProvider
{
    public const string Zoom = "Zoom";
    public const string Teams = "Teams";
    public const string GoogleMeet = "GoogleMeet";
    public const string WebEx = "WebEx";
    
    public static readonly string[] SupportedProviders = [Zoom, Teams, GoogleMeet, WebEx];
}

public static class CalendarIntegration
{
    public const string GoogleCalendar = "GoogleCalendar";
    public const string OutlookCalendar = "OutlookCalendar";
    public const string AppleCalendar = "AppleCalendar";
    public const string None = "None";
    
    public static readonly string[] SupportedIntegrations = [GoogleCalendar, OutlookCalendar, AppleCalendar, None];
}

public static class BandwidthOptimization
{
    public const string Low = "Low";
    public const string Medium = "Medium";
    public const string High = "High";
    public const string Auto = "Auto";
    
    public static readonly string[] SupportedOptimizations = [Low, Medium, High, Auto];
}