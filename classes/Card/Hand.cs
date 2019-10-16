﻿using System.Collections.Generic;
using System.Linq;

namespace Sulimn.Classes.Card
{
    /// <summary>Represents a hand of playing cards.</summary>
    internal class Hand
    {
        private List<Card> _cardList = new List<Card>();

        /// <summary>Actual value of Cards in Hand.</summary>
        /// <returns>Actual value</returns>
        internal int ActualValue => _cardList.Sum(card => card.Value);

        /// <summary>Total value of Cards in Hand.</summary>
        /// <returns>Total value</returns>
        internal int TotalValue => _cardList.Where(card => !card.Hidden).Sum(card => card.Value);

        #region Properties

        /// <summary>List of cards in the hand.</summary>
        public List<Card> CardList { get; set; }

        /// <summary>Current value of the Hand.</summary>
        public string Value => $"Total: {TotalValue}";

        #endregion Properties

        #region Hand Management

        /// <summary>Adds a Card to the Hand.</summary>
        /// <param name="newCard">Card to be added.</param>
        internal void AddCard(Card newCard) => CardList.Add(newCard);

        /// <summary>Clears the Hidden state of all Cards in the Hand.</summary>
        internal void ClearHidden()
        {
            foreach (Card card in CardList)
                card.Hidden = false;
        }

        /// <summary>Converts an 11-valued Ace to be valued at 1.</summary>
        internal void ConvertAce()
        {
            foreach (Card card in CardList)
                if (card.Value == 11)
                {
                    card.Value = 1;
                    break;
                }
        }

        #endregion Hand Management

        #region Constructors

        /// <summary>Initializes a default instance of Hand.</summary>
        internal Hand()
        {
        }

        /// <summary>Initializes an instance of Hand by assigning the list of Cards.</summary>
        /// <param name="cardList">List of Cards</param>
        internal Hand(List<Card> cardList) => CardList = cardList;

        #endregion Constructors
    }
}