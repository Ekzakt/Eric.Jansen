using Ej.Application.Contracts;
using Ej.Application.Validators;
using Ej.Client.Configuration;
using Ej.Client.Middlewares;
using Ej.Infrastructure.BackgroundServices;
using Ej.Infrastructure.Constants;
using Ej.Infrastructure.Queueing;
using Ej.Infrastructure.ScopedServices;
using Ej.Infrastructure.Services;
using Ekzakt.EmailSender.Smtp.Configuration;
using Ekzakt.EmailTemplateProvider.Io.Configuration;
using Ekzakt.FileManager.AzureBlob.Configuration;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.AddOpenTelemetry();
builder.AddRequestLocalization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews()
                .AddViewLocalization();

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
builder.Services.AddScoped<ICultureManager, CultureManager>();

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

var cultureOptions = app.Services.GetRequiredService<IOptions<CultureOptions>>().Value;
var requestLocalizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;

app.UseRequestLocalization(new RequestLocalizationOptions()
    .SetDefaultCulture(cultureOptions!.DefaultCulture!.Name)
    .AddSupportedCultures(cultureOptions!.SupportedCultures!.Select(c => c.Name).ToArray())
    .AddSupportedUICultures(cultureOptions!.SupportedCultures!.Select(c => c.Name).ToArray()));

app.UseMiddleware<TenantDetectorMiddleware>();

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
        context.Request.Path = $"/{CultureInfo.CurrentCulture.Name}/error/404";
        await next();
    }
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default", 
    pattern: "{culture=en-us}/{controller=Home}/{action=Index}/{id?}",
    constraints: new { culture = new CultureRouteConstraint() });

app.UseMiddleware<RedirectionMiddleWare>();

app.Run();
