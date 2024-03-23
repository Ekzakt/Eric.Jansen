using Ekzakt.EmailSender.Core.Models;
using Ekzakt.EmailSender.Core.Models.Responses;
using Ekzakt.EmailTemplateProvider.Core.Models;
using Ekzakt.Utilities;

namespace Eric.Jansen.Infrastructure.Services;

public abstract class AbstractEmailSenderService
{
    public List<EmailAddress> Tos { get; set; } = [];
    public List<EmailAddress> Ccs { get; set; } = [];
    public List<EmailAddress> Bccs { get; set; } = [];


    public async Task SendEmailAsync(string templateName, string cultureName, List<EmailAddress>? toUsers = null, StringReplacer? stringReplacer = null)
    {
        var templates = await GetTemplatesAsync(templateName, cultureName);

        foreach (var t in templates ?? [])
        {
            var template = ApplyTemplateReplacements(t, stringReplacer);

            var sendResponse = await SendEmailAsync(template);
        }
    }


    protected abstract Task<List<EmailTemplate>?> GetTemplatesAsync(string templateName, string cultureName);

    protected abstract EmailTemplate ApplyTemplateReplacements(EmailTemplate template, StringReplacer? stringReplacer);

    protected abstract Task<Guid?> SaveEmailAsync(EmailTemplate templates);

    protected abstract Task<SendEmailResponse> SendEmailAsync(EmailTemplate template);

    protected abstract Task UpdateStatusAsync(Guid? id, bool isSuccess);
}
