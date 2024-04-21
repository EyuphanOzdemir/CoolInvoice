using System.Threading.Tasks;

namespace InvoicrInfrastructure.Email
{
  public interface IEmailSender
  {
    Task SendEmailAsync(string to, string from, string subject, string body);
  }
}
