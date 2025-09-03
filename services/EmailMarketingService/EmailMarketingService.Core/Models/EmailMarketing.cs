using System.ComponentModel.DataAnnotations;

namespace EmailMarketingService.Core.Models;

public class EmailMarketing
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public string AccountId { get; set; } = string.Empty;
    
    [Required]
    public string EmailDeliveryInfra { get; set; } = string.Empty; // SES, Mailchimp
    
    [Required]
    public string EmailTemplate { get; set; } = string.Empty;
    
    [Required]
    public string Analytics { get; set; } = string.Empty;
    
    [Required]
    public string ABTesting { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}