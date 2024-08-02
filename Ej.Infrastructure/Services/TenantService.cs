using Ej.Application.Constants;
using Ej.Application.Contracts;
using Ej.Application.Models;

namespace Ej.Infrastructure.Services;

public class TenantService : ITenantService
{
    private readonly List<Tenant> _tenantList = [];

    public TenantService()
    {
        InitTenantList();
    }


    public Tenant GetByHostName(string hostName)
    {
        var output = _tenantList.FirstOrDefault(t => t.HostName.StartsWith(hostName));

        if (output is null)
        {
            output = _tenantList.First();
        }

        return output;
    }


    #region Helpers

    private void InitTenantList()
    {
        _tenantList.Add(new Tenant
        {
            HostName = "ericjansen.com",
            Name = "Eric Jansen",
            ShortName = "Eric",
            OpenGraphTags = new Dictionary<string, string>
            {
                { OpenGraphTags.TYPE, "website" },
                { OpenGraphTags.TITLE, "Eric Jansen" },
                { OpenGraphTags.SITE_NAME, "ericjansen.com" },
                { OpenGraphTags.URL, "https://ericjansen.com" },
                { OpenGraphTags.DESCRIPTION, "This is the personal website of Eric Jansen and serves as as his minimalistic online presence." },
                { OpenGraphTags.LOCALE, "en_US" },
                { OpenGraphTags.IMAGE, "https://ericjansen.com/img/face-headphones-original-1000x1000.jpg" },
                { OpenGraphTags.IMAGE_HEIGHT, "1000" },
                { OpenGraphTags.IMAGE_WIDTH, "1000" }
            }
        });

        _tenantList.Add(new Tenant
        {
            HostName = "rixke.be",
            Name = "Rixke",
            ShortName = "Rixke",
            OpenGraphTags = new Dictionary<string, string>
            {
                { OpenGraphTags.TYPE, "website" },
                { OpenGraphTags.TITLE, "Rixke" },
                { OpenGraphTags.SITE_NAME, "rixke.be" },
                { OpenGraphTags.URL, "https://rixke.be" },
                { OpenGraphTags.DESCRIPTION, "This is the personal website of Rixke and serves as as his minimalistic online presence." },
                { OpenGraphTags.LOCALE, "en_US" },
                { OpenGraphTags.IMAGE, "https://rixke.be/img/face-headphones-original-1000x1000.jpg" },
                { OpenGraphTags.IMAGE_HEIGHT, "1000" },
                { OpenGraphTags.IMAGE_WIDTH, "1000" }
            }
        });
    }

    #endregion Helpers
}