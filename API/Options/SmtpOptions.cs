using System.ComponentModel.DataAnnotations;

namespace RunnymedeScouts.API.Options;

public class SmtpOptions
{
    [Required]
    public required string Host { get; set; }

    [Required]
    public required int Port { get; set; }

    [Required]
    public required string Username { get; set; }

    [Required]
    public required string DisplayName { get; set; }

    [Required]
    public required string Domain { get; set; }

    [Required]
    public required string Password { get; set; }
}
