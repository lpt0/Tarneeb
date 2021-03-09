using System;
using System.Collections.Generic;
using System.Text;

/**
 * @author: Haran
 * @date: 2021-03-01
 */
namespace TarneebClasses.Events
{
    /// <summary>
    /// Helper functions related to event functionality.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Raise an event with the specified arguments.
        /// </summary>
        /// <param name="handler">The event to raise.</param>
        /// <param name="sender">The object that is raising the event.</param>
        /// <param name="args">The arguments for the event.</param>
        //public static void RaiseEvent(EventHandler<EventArgs> handler, object sender, EventArgs args)
        //{
        //    // If there are any listeners
        //    if (handler != null)
        //    {
        //        // Raise the event
        //        handler(sender, args);
        //    }
        //}

        public static void RaiseEvent(EventHandler<NewGameEventArgs> handler, object sender, NewGameEventArgs args) => RaiseEvent(handler, sender, args);

        public static void RaiseEvent(EventHandler<PlayerPlaceBidEventArgs> handler, object sender, PlayerPlaceBidEventArgs args)
            => handler?.Invoke(sender, args);
    }
}
