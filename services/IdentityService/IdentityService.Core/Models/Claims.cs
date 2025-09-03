namespace IdentityService.Core.Models;

public static class Claims
{
    public static class Customer
    {
        public const string EmailMarketingUpdate = "EmailMarketing.Update";
        public const string TextNotificationUpdate = "TextNotification.Update";
        public const string VideoConferencingUpdate = "VideoConferencing.Update";
    }
    
    public static class Support
    {
        public const string EmailMarketingUpdate = "EmailMarketing.Update";
        public const string TextNotificationUpdate = "TextNotification.Update";
        public const string VideoConferencingUpdate = "VideoConferencing.Update";
        public const string EmailMarketingDelete = "EmailMarketing.Delete";
        public const string TextNotificationDelete = "TextNotification.Delete";
        public const string VideoConferencingDelete = "VideoConferencing.Delete";
    }
    
    public static string[] GetClaimsForRole(UserRole role)
    {
        return role switch
        {
            UserRole.Customer => [Customer.EmailMarketingUpdate, Customer.TextNotificationUpdate, Customer.VideoConferencingUpdate],
            UserRole.Support => [Support.EmailMarketingUpdate, Support.TextNotificationUpdate, Support.VideoConferencingUpdate, 
                               Support.EmailMarketingDelete, Support.TextNotificationDelete, Support.VideoConferencingDelete],
            _ => []
        };
    }
}