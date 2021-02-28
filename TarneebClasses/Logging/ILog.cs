using System;
using System.Collections.Generic;
using System.Text;

/**
 * Author: Haran
 * Date: 2021-02-08
 */
namespace TarneebClasses.Logging
{
    /// <summary>
    /// Generic game log.
    /// </summary>
    interface ILog
    {
        /// <summary>
        /// The type of log.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// The date and time that the log was created.
        /// </summary>
        DateTime DateTime { get; }

        /// <summary>
        /// Get this log as a string.
        /// </summary>
        /// <returns>A string representation of the log.</returns>
        string ToString();
    }
}
