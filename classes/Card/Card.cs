using Godot;
using Sulimn.Classes.Enums;
using System;

namespace Sulimn.Classes.Card
{
    /// <summary>Represents a playing card.</summary>
    internal class Card : TextureRect, IEquatable<Card>
    {
        #region Properties

        /// <summary>The suit of the card.</summary>
        public CardSuit Suit { get; set; }

        /// <summary>The value of the card.</summary>
        public int Value { get; set; }

        /// <summary>Should the Card be hidden from the player?</summary>
        public bool Hidden { get; set; }

        /// <summary>Returns the name and suit of the card.</summary>
        public string CardToString => $"{Name}_of_{Suit}".ToLower();

        #endregion Properties

        /// <summary>Hides the card and sets the texture.</summary>
        public void HideCard()
        {
            Hidden = true;
            SetTexture();
        }

        public void SetTexture() => Texture = Hidden ? (Texture)ResourceLoader.Load("res://assets/cards/back.png") : (Texture)ResourceLoader.Load($"res://assets/cards/{CardToString}.png");

        #region Override Operators

        public static bool Equals(Card left, Card right)
        {
            if (left is null && right is null) return true;
            if (left is null ^ right is null) return false;
            return string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase)
                   && left.Suit == right.Suit
                   && left.Value == right.Value && left.Hidden == right.Hidden;
        }

        public sealed override bool Equals(object obj) => Equals(this, obj as Card);

        public bool Equals(Card other) => Equals(this, other);

        public static bool operator ==(Card left, Card right) => Equals(left, right);

        public static bool operator !=(Card left, Card right) => !Equals(left, right);

        public sealed override int GetHashCode() => base.GetHashCode() ^ 17;

        public sealed override string ToString() => CardToString;

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a default instance of Card.</summary>
        internal Card()
        {
        }

        /// <summary>Initializes an instance of Card by assigning Properties.</summary>
        /// <param name="name">Name of Card</param>
        /// <param name="suit">Suit of Card</param>
        /// <param name="value">Value of Card</param>
        /// <param name="hidden">Should the Card be hidden from the player?</param>
        internal Card(string name, CardSuit suit, int value, bool hidden)
        {
            Name = name;
            Suit = suit;
            Value = value;
            Hidden = hidden;
        }

        /// <summary>Replaces this instance of Card with another instance.</summary>
        /// <param name="other">Instance to replace this instance</param>
        internal Card(Card other) : this(other.Name, other.Suit, other.Value, other.Hidden)
        {
        }

        /// <summary>Replaces this instance of Card with another instance.</summary>
        /// <param name="other">Instance to replace this instance</param>
        /// <param name="hidden">Should the Card be hidden from the player?</param>
        internal Card(Card other, bool hidden) : this(other.Name, other.Suit, other.Value, hidden)
        {
        }

        #endregion Constructors
    }
}