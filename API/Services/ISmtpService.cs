namespace RunnymedeScouts.API.Services;

public interface ISmtpService
{
    bool SendEmail(string email, string name, string subject, string message);
}