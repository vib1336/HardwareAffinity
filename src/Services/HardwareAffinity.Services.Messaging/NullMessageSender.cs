namespace HardwareAffinity.Services.Messaging
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.UI.Services;

    public class NullMessageSender : IEmailSender
    {
        public Task SendEmailAsync(
            string email,
            string subject,
            string message)
        {
            return Task.CompletedTask;
        }
    }
}
