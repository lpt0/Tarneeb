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
    /// A log representing the start of a new game.
    /// </summary>
    public class NewGameLog : ILog
    {
        public Type Type => Type.NEW_GAME;

        public DateTime DateTime => DateTime.Now;

        public override string ToString()
        {
            return $"New game started.";
        }
    }
}
