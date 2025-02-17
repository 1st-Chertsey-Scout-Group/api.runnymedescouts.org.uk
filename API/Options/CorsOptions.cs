namespace RunnymedeScouts.API.Options;

public class CorsOptions
{
    public required string AllowedOrigins { get; set; }
    public required string AllowedHeaders { get; set; }
    public required string AllowedMethods { get; set; }
    public required string AllowedHosts { get; set; }


    public string[] AllowedOriginsList
    {
        get
        {
            if (string.IsNullOrEmpty(AllowedOrigins))
            {
                return [];
            }

            return AllowedOrigins.Split(',');
        }
    }

    public string[] AllowedHeadersList
    {
        get
        {
            if (string.IsNullOrEmpty(AllowedHeaders))
            {
                return [];
            }

            return AllowedHeaders.Split(',');
        }
    }

    public string[] AllowedMethodsList
    {
        get
        {
            if (string.IsNullOrEmpty(AllowedMethods))
            {
                return [];
            }
            return AllowedMethods.Split(',');
        }
    }
}