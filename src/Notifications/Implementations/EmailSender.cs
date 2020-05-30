namespace Miniblog.Core.Notifications.Implementations
{
    using System;
    using System.Threading.Tasks;
    using MailKit.Net.Smtp;
    using Microsoft.Extensions.Configuration;
    using MimeKit;

    /// <inheritdoc cref="IEmailSender"/>
    public sealed class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSender"/> class.
        /// </summary>
        private EmailSender(IConfiguration configuration) => this.configuration = configuration;

        /// <inheritdoc />
	    public Task SendAsync(
            string recipient,
            string subject,
            string message)
        {
            var smtpHost = this.configuration[Constants.Config.Notification.Email.Host];
            var smtpPort = int.TryParse(this.configuration[Constants.Config.Notification.Email.Port], out var port) ? port : -1;
            var userName = this.configuration[Constants.Config.Notification.Email.UserName];
            var password = this.configuration[Constants.Config.Notification.Email.Password];
            var name = this.configuration[Constants.Config.Blog.Name] ?? "Miniblog";

            if (string.IsNullOrWhiteSpace(smtpHost))
            {
                throw new ArgumentException($"The address of SMTP server must be specified. Please check settings in the configuration file.");
            }

            if (smtpPort <= -1)
            {
                throw new ArgumentException($"The port of SMTP server must be specified. Please check settings in the configuration file.");
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException($"The username must be specified. Please check settings in the configuration file.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException($"The password must be specified. Please check settings in the configuration file.");
            }

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(name, userName));
            emailMessage.To.Add(new MailboxAddress(name, recipient));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.ConnectAsync(smtpHost, smtpPort, false);
                client.AuthenticateAsync(userName, password);
                client.SendAsync(emailMessage);

                client.DisconnectAsync(true);
            }

            return Task.CompletedTask;
        }
    }
}
