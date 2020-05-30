namespace Miniblog.Core
{
    /// <summary>
    /// Responsible for notification settings.
    /// </summary>
    public class NotificationSettings
    {
        /// <summary>
        /// Flag indicating whether the message sending function is enabled (default: false).
        /// </summary>
	    public bool IsEmailEnabled { get; set; } = false;

        /// <summary>
        /// The email settings.
        /// </summary>
        public EmailSettings Email { get; set; }
    }

    /// <summary>
    /// Responsible for email settings.
    /// </summary>
    public class EmailSettings
    {
	    /// <summary>
	    /// The address of host.
	    /// </summary>
	    public string Host { get; set; }

	    /// <summary>
	    /// The port.
	    /// </summary>
	    public int Port { get; set; }

	    /// <summary>
	    /// A flag indicating whether it is necessary to use ssl (default: false).
	    /// </summary>
	    public bool Ssl { get; set; } = false;

	    /// <summary>
	    /// The username of sender.
	    /// </summary>
	    public string UserName { get; set; }

	    /// <summary>
	    /// The password.
	    /// </summary>
	    public string Password { get; set; }
    }
}
