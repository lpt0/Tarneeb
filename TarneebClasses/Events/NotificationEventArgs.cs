using System;
using System.Collections.Generic;
using System.Text;

/**
 * @author: Haran
 * @date: 2021-02-28
 */
namespace TarneebClasses.Events
{
    /// <summary>
    /// Event arguments for notifications.
    /// </summary>
    /// <see cref="EventArgs" />
    public class NotificationEventArgs : EventArgs
    {
        /// <summary>
        /// The notification message.
        /// </summary>
        public string Message { get; set; }
    }
}
