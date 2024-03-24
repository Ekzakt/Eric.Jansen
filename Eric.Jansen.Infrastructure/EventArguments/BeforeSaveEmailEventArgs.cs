using Ekzakt.EmailTemplateProvider.Core.Models;

namespace Eric.Jansen.Infrastructure.EventArguments;

public class BeforeSaveEmailEventArgs : EventArgs
{
    public Guid Id { get; init; }

    public EmailTemplate? EmailTemplate { get; init; }
}
