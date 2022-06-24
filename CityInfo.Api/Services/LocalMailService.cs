namespace CityInfo.Api.Services;

public class LocalMailService : IMailService
{
    private readonly string _mailTo = string.Empty;
    private readonly string _mailFrom = string.Empty;

    // IConfiguration is automatically injected via asp.netcore
    public LocalMailService(IConfiguration configuration)
    {
        // These are in applicationsettings.json
        _mailTo = configuration["mailSettings:mailToAddress"];
        _mailFrom = configuration["mailSettings:mailFromAddress"];
    }
    public void Send(string subject, string message)
    {
        // send the mail = output to console windoow
        Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}" +
                          $"with {nameof(LocalMailService)}." +
                          $"\n Subject: {subject}" +
                          $"\n Message: {message}");
    }
}