using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Sulimn.Classes.Card
{
    /// <summary>Represents a hand of playing cards.</summary>
    internal class Hand
    {
        /// <summary>Actual value of <see cref="Card"/>s in <see cref="Hand"/>.</summary>
        /// <returns>Actual value</returns>
        internal int ActualValue => CardList.Sum(card => card.Value);

        /// <summary>Count of <see cref="Card"/>s in the <see cref="Hand"/>.</summary>
        internal int Count => CardList.Count;

        /// <summary>Total value of <see cref="Card"/>s in <see cref="Hand"/>.</summary>
        /// <returns>Total value</returns>
        internal int TotalValue => CardList.Where(card => !card.Hidden).Sum(card => card.Value);

        #region Properties

        /// <summary>List of <see cref="Card"/>s in the <see cref="Hand"/>.</summary>
        public List<Card> CardList { get; set; } = new List<Card>();

        /// <summary>Current value of the <see cref="Hand"/>.</summary>
        public string Value => $"Total: {TotalValue}";

        #endregion Properties

        #region Hand Management

        /// <summary>Adds a <see cref="Card"/> to the <see cref="Hand"/>.</summary>
        /// <param name="newCard">Card to be added.</param>
        internal void AddCard(Card newCard)
        {
            newCard.Name = newCard.CardToString;
            newCard.SetTexture();
            newCard.RectPosition = new Vector2(CardList.Count * 40f, 0f);
            CardList.Add(newCard);
        }

        /// <summary>Can the <see cref="Hand"/> be split?</summary>
        /// <returns>True if <see cref="Hand"/> can be split</returns>
        internal bool CanSplit() => CardList.Count == 2 && CardList[0].CardName == CardList[1].CardName && CardList[0].Value == CardList[1].Value;

        /// <summary>Checks whether the <see cref="Hand"/> can be Doubled Down.</summary>
        /// <returns>Returns true if <see cref="Hand"/> can be Doubled Down.</returns>
        internal bool CanDoubleDown() => CardList.Count == 2 && TotalValue >= 9 && TotalValue <= 11;

        /// <summary>Clears the Hidden state of all <see cref="Card"/>s in the <see cref="Hand"/>.</summary>
        internal void ClearHidden()
        {
            CardList.ForEach(card => card.Hidden = false);
            CardList.ForEach(card => card.SetTexture());
        }

        /// <summary>Converts an 11-valued Ace <see cref="Card"/> to be valued at 1.</summary>
        internal void ConvertAce()
        {
            foreach (Card card in CardList)
                if (card.Value == 11)
                {
                    card.Value = 1;
                    break;
                }
        }

        /// <summary>Checks whether a <see cref="Hand"/> has an Ace valued at eleven in it.</summary>
        /// <returns>Returns true if <see cref="Hand"/> has an Ace valued at eleven in it.</returns>
        internal bool HasAceEleven() => CardList.Any(card => card.Value == 11);

        /// <summary>Checks whether a <see cref="Hand"/> has reached Blackjack.</summary>
        /// <returns>Returns true if the <see cref="Hand"/>'s value is 21.</returns>
        internal bool HasBlackjack() => TotalValue == 21;

        /// <summary>Checks whether the current <see cref="Hand"/> has gone Bust.</summary>
        /// <returns>Returns true if <see cref="Hand"/> has gone Bust.</returns>
        internal bool IsBust() => !HasAceEleven() && TotalValue > 21;

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