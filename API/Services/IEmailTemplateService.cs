using RunnymedeScouts.API.Enums;

namespace RunnymedeScouts.API.Services;

public interface IEmailTemplateService
{
    public string GetTemplate(EmailTemplateTypeEnum type, Dictionary<string, string> tokens);
}
