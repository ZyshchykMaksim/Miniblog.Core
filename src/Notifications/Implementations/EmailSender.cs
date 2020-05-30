namespace Miniblog.Core.Notifications.Implementations
{
    /* NOTE:
     * If you use a gmail.com, then read the following information - https://support.google.com/mail/answer/7104828?hl=en&visit_id=637264509159051651-3131750136&rd=1
     * Also, You might get below error,"The SMTP server requires a secure connection or the client was not authenticated. The server response was: 5.5.1 Authentication Required."
     * To fix you need to enable "Allow less secure apps: ON". This option is only available if the 2-Step Verification function is disabled.
     */

    using System;
    using System.Threading.Tasks;
    using MailKit.Net.Smtp;
    using Microsoft.Extensions.Options;
    using MimeKit;

    /// <inheritdoc cref="IEmailSender"/>
    public sealed class EmailSender : IEmailSender
    {
        private readonly IOptionsSnapshot<EmailSettings> emailSettings;
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSender"/> class.
        /// </summary>
        public EmailSender(IOptionsSnapshot<EmailSettings> emailSettings) => this.emailSettings = emailSettings;

        /// <inheritdoc />
	    public async Task SendAsync(
            string recipient,
            string subject,
            string message)
        {
            if (string.IsNullOrWhiteSpace(this.emailSettings.Value.Host))
            {
                throw new ArgumentException($"The address of SMTP server must be specified. Please check settings in the configuration file.");
            }

            if (this.emailSettings.Value.Port <= -1)
            {
                throw new ArgumentException($"The port of SMTP server must be specified. Please check settings in the configuration file.");
            }

            if (string.IsNullOrWhiteSpace(this.emailSettings.Value.UserName))
            {
                throw new ArgumentException($"The username must be specified. Please check settings in the configuration file.");
            }

            if (string.IsNullOrWhiteSpace(this.emailSettings.Value.Password))
            {
                throw new ArgumentException($"The password must be specified. Please check settings in the configuration file.");
            }

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("", this.emailSettings.Value.UserName));
            emailMessage.To.Add(new MailboxAddress("", recipient));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(
                    this.emailSettings.Value.Host,
                    this.emailSettings.Value.Port,
                    this.emailSettings.Value.Ssl).ConfigureAwait(false);

                await client.AuthenticateAsync(
                    this.emailSettings.Value.UserName,
                    this.emailSettings.Value.Password).ConfigureAwait(false);

                await client.SendAsync(emailMessage).ConfigureAwait(false);

                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}
