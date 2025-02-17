using RunnymedeScouts.API.Enums;

namespace RunnymedeScouts.API.Services;

public class EmailTemplateService : IEmailTemplateService
{

    public Dictionary<EmailTemplateTypeEnum, string> Templates { get; set; }

    public EmailTemplateService()
    {
        Templates = new Dictionary<EmailTemplateTypeEnum, string>();
        AddTemplates();
    }


    public void AddTemplates()
    {
        foreach (EmailTemplateTypeEnum type in (EmailTemplateTypeEnum[])Enum.GetValues(typeof(EmailTemplateTypeEnum)))
        {

            string templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
            string filePath = Path.Combine(templateFolderPath, $"{type}.html");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Email template file not found: {filePath}");
            }

            Templates.Add(type, File.ReadAllText(filePath));
        }
    }

    public string GetTemplate(EmailTemplateTypeEnum type, Dictionary<string, string> tokens)
    {
        if (!Templates.ContainsKey(type))
        {
            throw new Exception($"Unknown Email Template Type: {type}");
        }

        var template = Templates.First(t => t.Key == type).Value;

        foreach (var key in tokens.Keys)
        {
            template = template.Replace($"#{key}#", tokens[key]);
        }

        return template;
    }
}
