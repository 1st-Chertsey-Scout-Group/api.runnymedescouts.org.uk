using System.ComponentModel.DataAnnotations;

namespace RunnymedeScouts.API.DTOs;

public class PostEnquiryDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not a valid email address")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Subject is required")]
    public required string Subject { get; set; }

    [Required(ErrorMessage = "Message is required")]
    public required string Message { get; set; }

    public string? Number { get; set; }
}
