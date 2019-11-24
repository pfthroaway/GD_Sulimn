using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Sulimn.Classes.Card
{
    /// <summary>Represents a hand of playing cards.</summary>
    internal class Hand
    {
        /// <summary>Actual value of Cards in Hand.</summary>
        /// <returns>Actual value</returns>
        internal int ActualValue => CardList.Sum(card => card.Value);

        /// <summary>Total value of Cards in Hand.</summary>
        /// <returns>Total value</returns>
        internal int TotalValue => CardList.Where(card => !card.Hidden).Sum(card => card.Value);

        #region Properties

        /// <summary>List of cards in the hand.</summary>
        public List<Card> CardList { get; set; } = new List<Card>();

        /// <summary>Current value of the Hand.</summary>
        public string Value => $"Total: {TotalValue}";

        #endregion Properties

        #region Hand Management

        /// <summary>Adds a Card to the Hand.</summary>
        /// <param name="newCard">Card to be added.</param>
        internal void AddCard(Card newCard)
        {
            newCard.SetTexture();
            newCard.RectPosition = new Vector2(CardList.Count * 40f, 0f);
            CardList.Add(newCard);
        }

        /// <summary>Can the Hand be split?</summary>
        /// <returns>True if Hand can be split</returns>
        internal bool CanSplit() => (CardList.Count == 2 && CardList[0].CardName == CardList[1].CardName && CardList[0].Value == CardList[1].Value);

        /// <summary>Clears the Hidden state of all Cards in the Hand.</summary>
        internal void ClearHidden()
        {
            CardList.ForEach(card => card.Hidden = false);
            CardList.ForEach(card => card.SetTexture());
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