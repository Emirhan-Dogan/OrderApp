using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Core.Utilities.Mail;
using MimeKit.Text;
using System;
using System.Linq;

public class MailManager : IMailService
{
    private readonly IConfiguration _configuration;

    public MailManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Send(EmailMessage emailMessage)
    {
        var message = new MimeMessage();
        message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
        message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
        message.Subject = emailMessage.Subject;

        // Sadece emailMessage.Content'i kullan
        var messageBody = emailMessage.Content;
        message.Body = new TextPart(TextFormat.Html) { Text = messageBody };

        using var emailClient = new SmtpClient();
        try
        {
            // SMTP sunucusuna bağlan
            emailClient.Connect(
                _configuration.GetSection("EmailConfiguration").GetSection("SmtpServer").Value,
                Convert.ToInt32(_configuration.GetSection("EmailConfiguration").GetSection("SmtpPort").Value),
                SecureSocketOptions.StartTls);

            // Kimlik doğrulama
            emailClient.Authenticate(
                _configuration.GetSection("EmailConfiguration").GetSection("Username").Value,
                _configuration.GetSection("EmailConfiguration").GetSection("Password").Value);

            // E-postayı gönder
            emailClient.Send(message);
            emailClient.Disconnect(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"E-posta gönderme hatası: {ex.Message}");
        }
    }

}
