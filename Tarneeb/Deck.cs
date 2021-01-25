using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * @author  Andrew Kuo
 * @date    2021-01-22
 */

namespace Tarneeb
{
    /// <summary>
    /// Deck class represents a collection of cards. It derived from Collection
    /// List<> allowing access to useful methods provided the Collection-based
    /// classes.
    /// 
    /// Details on List<T> Class:
    ///     system.collections.generic.list
    /// 
    /// </summary>
    class Deck : List<Card>
    {
        /// <summary>
        /// Deck's Human readable label.
        /// </summary>
        public string DeckName { get; set; }

        /// <summary>
        /// Deck's unique ID.
        /// </summary>
        public int DeckId { get; set; }

        /// <summary>
        /// Returns summary of the Deck as a string.
        /// 
        /// TODO: Perhaps the to String should return name with number of
        /// cards.
        /// </summary>
        /// <returns>Summary of Deck as String</returns>
        public override string ToString()
        {
            return "ID: " + DeckId + "  Name: " + DeckName + "  O:" + base.ToString();
        }

        /// <summary>
        /// Checks if the given object is a the same.
        /// </summary>
        /// <param name="obj">Test object.</param>
        /// <returns>If object are equals</returns>
        public override bool Equals(object obj)
        {
            // Check if there is an object.
            if (obj == null)
            {
                return false;
            }
            // Cast the obj as a Deck
            Deck potentialDeck = obj as Deck;
            // Check if failed to be cast.
            if (potentialDeck == null)
            {
                return false;
            }
            // Call the base classes Equals method.
            return base.Equals(potentialDeck);
        }

        /// <summary>
        /// Equals method to check other Decks. Simply calls 
        /// Equals(object obj).
        /// </summary>
        /// <param name="other">Another Deck.</param>
        /// <returns>If the two object match.</returns>
        public bool Equals(Deck other)
        {
            // Check if provided Deck is null.
            if (other == null)
            {
                return false;
            }
            // Else use the Deck's Equals.
            return this.DeckId.Equals(other.DeckId);
        }

        /// <summary>
        /// Returns the unique ID of the Deck.
        /// Does not actually return a hash.
        /// </summary>
        /// <returns>Integer representing the Deck.</returns>
        public override int GetHashCode()
        {
            //return base.GetHashCode();
            return DeckId;
        }

        // TODO: Sort Comparison? Need Card to implement values.

    }
}
