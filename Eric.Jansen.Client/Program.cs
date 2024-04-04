using Ekzakt.EmailSender.Smtp.Configuration;
using Ekzakt.EmailTemplateProvider.Io.Configuration;
using Ekzakt.FileManager.AzureBlob.Configuration;
using Eric.Jansen.Application.Validators;
using Eric.Jansen.Client.Configuration;
using Eric.Jansen.Infrastructure.BackgroundServices;
using Eric.Jansen.Infrastructure.Constants;
using Eric.Jansen.Infrastructure.Queueing;
using Eric.Jansen.Infrastructure.ScopedServices;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddEkzaktFileManagerAzure();
builder.Services.AddEkzaktEmailTemplateProviderIo();
builder.Services.AddEkzaktSmtpEmailSender();

builder.Services.AddTransient<IQueueService, QueueService>();

builder.Services.AddHostedService<ContactFormQueueBackgroundService>();
builder.Services.AddKeyedScoped<IScopedService, ContactFormService>(ProcessingServiceKeys.CONTACT_FORM);

builder.Services.AddKeyedScoped<IScopedService, EmailService>(ProcessingServiceKeys.EMAILS);
builder.Services.AddHostedService<EmailBgService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddValidatorsFromAssemblyContaining<ContactViewModelValidator>();

builder.Services.AddProblemDetails();

builder.AddEricJansenOptions();
builder.AddAzureClientServices();
builder.AddAzureKeyVault();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/error/404";
        await next();
    }
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
