using Application.IServices;
using Application.ValueObjects.Email;
using MailKit.Net.Smtp;
using MimeKit;

namespace Infrastructure.Services;
public class EmailService : IEmailService
{
    private const string From = "cuongnpm116@gmail.com";
    private const string SmtpServer = "smtp.gmail.com";
    private const int Port = 587;
    private const string Password = "zavyziykncyxjyuz";

    public EmailService()
    {
    }

    public async Task SendEmail(Message msg)
    {
        var emailMsg = CreateEmailMessage(msg);
        await SendAsync(emailMsg);
    }

    public async Task SendEmailWithHtml(Message msg)
    {
        var emailMsg = CreateEmailWithHtmlMessage(msg);
        await SendAsync(emailMsg);
    }

    private MimeMessage CreateEmailWithHtmlMessage(Message msg)
    {
        var emailMsg = new MimeMessage();
        emailMsg.From.Add(new MailboxAddress("CoolSharp", From));
        emailMsg.To.AddRange(msg.To);
        emailMsg.Subject = msg.Subject;
        emailMsg.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = msg.Content,
        };

        return emailMsg;
    }

    private MimeMessage CreateEmailMessage(Message msg)
    {
        var emailMsg = new MimeMessage();
        emailMsg.From.Add(new MailboxAddress("CoolSharp", From));
        emailMsg.To.AddRange(msg.To);
        emailMsg.Subject = msg.Subject;
        emailMsg.Body = new TextPart(MimeKit.Text.TextFormat.Text)
        {
            Text = msg.Content,
        };


        return emailMsg;
    }

    private async Task SendAsync(MimeMessage mailMsg)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(SmtpServer, Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(From, Password);
            await client.SendAsync(mailMsg);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
        }
    }
}
