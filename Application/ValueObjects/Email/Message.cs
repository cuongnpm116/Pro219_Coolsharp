using MimeKit;

namespace Application.ValueObjects.Email;
public class Message
{
    public List<MailboxAddress> To { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public Message(IEnumerable<string> to, string subject, string content)
    {
        To = [];
        To.AddRange(to.Select(x => new MailboxAddress("CoolSharp", x)));
        Subject = subject;
        Content = content;
    }
}
