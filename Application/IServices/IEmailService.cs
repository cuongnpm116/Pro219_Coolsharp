using Application.ValueObjects.Email;

namespace Application.IServices;
public interface IEmailService
{
    Task SendEmail(Message msg);
    Task SendEmailWithHtml(Message msg);
}
