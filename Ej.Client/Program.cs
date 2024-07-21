using Ekzakt.EmailSender.Smtp.Configuration;
using Ekzakt.EmailTemplateProvider.Io.Configuration;
using Ekzakt.FileManager.AzureBlob.Configuration;
using Ej.Application.Contracts;
using Ej.Application.Validators;
using Ej.Client.Configuration;
using Ej.Client.Extensions;
using Ej.Infrastructure.BackgroundServices;
using Ej.Infrastructure.Constants;
using Ej.Infrastructure.Queueing;
using Ej.Infrastructure.ScopedServices;
using Ej.Infrastructure.Services;
using FluentValidation;
using Microsoft.Extensions.Options;
using Ej.Application.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddOpenTelemetry();
builder.AddRequestLocalization();

builder.Services.AddControllersWithViews();
builder.Services.AddEkzaktFileManagerAzure();
builder.Services.AddEkzaktEmailTemplateProviderIo();
builder.Services.AddEkzaktEmailSenderSmtp();

builder.Services.Configure<HostOptions>(options =>
{
    options.ServicesStartConcurrently = true;
    options.ServicesStopConcurrently = true;
});

builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IQueueService, QueueService>();

builder.Services.AddHostedService<ContactFormQueueBgService>();
builder.Services.AddHostedService<EmailBgService>();
builder.Services.AddKeyedScoped<IScopedService, ContactFormService>(ProcessingServiceKeys.CONTACT_FORM);
builder.Services.AddKeyedScoped<IScopedService, EmailService>(ProcessingServiceKeys.EMAILS);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<ContactViewModelValidator>();
builder.Services.AddProblemDetails();

builder.AddEricJansenOptions();
builder.AddAzureClientServices();
builder.AddAzureKeyVault();


var app = builder.Build();

var globalizationOptions = app.Services.GetRequiredService<IOptions<LocalizationOptions>>().Value;

app.UseRequestLocalization(new RequestLocalizationOptions()
    .SetDefaultCulture(globalizationOptions!.DefaultCulture!.Name)
    .AddSupportedCultures(globalizationOptions!.SupportedCultures!.Select(c => c.Name).ToArray())
    .AddSupportedUICultures(globalizationOptions!.SupportedCultures!.Select(c => c.Name).ToArray()));

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
app.UseTenantDetector();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default", 
    pattern: "{culture=en-US}/{controller=Home}/{action=Index}/{id?}",
    constraints: new { culture = new CultureRouteConstraint() });

app.Run();
