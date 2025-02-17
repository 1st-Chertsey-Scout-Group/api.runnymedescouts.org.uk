using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RunnymedeScouts.API.Options;

namespace RunnymedeScouts.API.Services;

public class SmtpService : ISmtpService
{
    private readonly ILogger<SmtpService> _logger;
    private readonly SmtpOptions _smtpOptions;
    private readonly IEmailTemplateService _emailTemplateService;
    public SmtpService(ILogger<SmtpService> logger, IOptions<SmtpOptions> smtpOptions, IEmailTemplateService emailTemplateService)
    {
        _logger = logger;
        _smtpOptions = smtpOptions.Value;
        _emailTemplateService = emailTemplateService;
    }

    public bool SendEmail(string email, string name, string subject, string message)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_smtpOptions.DisplayName, _smtpOptions.Username));
            emailMessage.To.Add(new MailboxAddress($"{subject}@{_smtpOptions.Domain}"));
            emailMessage.Subject = $"New Submission - {_smtpOptions.Domain}";

            var body = _emailTemplateService.GetTemplate(Enums.EmailTemplateTypeEnum.AdminEnquiry, new Dictionary<string, string>() {
                {"SUBJECT", Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(subject)},
                {"DOMAIN",_smtpOptions.Domain},
                {"NAME", name},
                {"EMAIL", email},
                {"MESSAGE", message},
            });

            emailMessage.Body = new TextPart("html") { Text = body };
            using var client = new SmtpClient();
            client.Connect(_smtpOptions.Host, _smtpOptions.Port, MailKit.Security.SecureSocketOptions.SslOnConnect);
            client.Authenticate(_smtpOptions.Username, _smtpOptions.Password);
            client.Send(emailMessage);
            client.Disconnect(true);

            _logger.LogInformation("Email sent to {email} successfully.", email);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error sending email to {email}: {ex.Message}", email, ex.Message);
            return false;
        }
    }
}
