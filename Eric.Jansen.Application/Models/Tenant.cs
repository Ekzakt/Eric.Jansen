namespace Eric.Jansen.Application.Models;

public class Tenant
{
    public Tenant(string hostName)
    {
        if (hostName.Contains("rixke"))
        {
            Title = "Rixke";
            Aka = "Eric Jansen";
            AkaUri = new Uri("https://ericjansen.com");
        }
    }

    public string Title { get; set; } = "Eric Jansen";
    public string Aka { get; set; } = "Rixke";
    public Uri AkaUri { get; set; } = new Uri("https://rixke.be");
}
