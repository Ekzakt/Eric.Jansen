namespace Ej.Client.Configuration;

public class CultureOptions
{
    public string DefaultCulture { get; set; } = "en-US";

    public string[] SupportedCultures { get; set; } = new[] { "en-US", "fr-FR" };
}
