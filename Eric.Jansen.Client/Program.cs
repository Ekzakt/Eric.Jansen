using Ekzakt.EmailSender.Smtp.Configuration;
using Ekzakt.EmailTemplateProvider.Io.Configuration;
using Ekzakt.FileManager.AzureBlob.Configuration;
using Eric.Jansen.Infrastructure.BackgroundServices;
using Eric.Jansen.Infrastructure.Queueing;
using Eric.Jansen.Infrastructure.Services;
using Eric.Jansen.Client.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddEkzaktSmtpEmailSender();
builder.Services.AddAzureBlobFileManager();
builder.Services.AddEkzaktEmailTemplateProviderIo();

builder.Services.AddScoped<EmailSenderService>();
builder.Services.AddScoped<QueueService>();
builder.Services.AddHostedService<ContactFormQueueBackgroundService>();


builder.AddAzureClientServices();
builder.AddAzureKeyVault();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
   
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePages();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(name: "contact", pattern: "{controller=Contact}/{action=Index}");

app.Run();
