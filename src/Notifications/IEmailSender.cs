using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miniblog.Core.Notifications
{
    /// <summary>
    /// ISender.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends the message to the recipient.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        /// <param name="subject">The subject of topic.</param>
        /// <param name="message">The message of topic.</param>
        /// <returns></returns>
        Task SendAsync(string recipient, string subject, string message);
    }
}
