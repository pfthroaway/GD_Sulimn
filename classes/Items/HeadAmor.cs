using Sulimn.Classes.HeroParts;
using System.Collections.Generic;

namespace Sulimn.Classes.Items
{
    /// <summary>Represents a piece of Armor worn on the head.</summary>
    internal class HeadArmor : Armor
    {
        #region Constructors

        /// <summary>Initializes a default instance of <see cref="HeadArmor">.</summary>
        internal HeadArmor()
        {
        }

        /// <summary>Initializes an instance of <see cref="HeadArmor"> by assigning Properties.</summary>
        /// <param name="name">Name of <see cref="HeadArmor"></param>
        /// <param name="description">Description of <see cref="HeadArmor"></param>
        /// <param name="defense">Defense of <see cref="HeadArmor"></param>
        /// <param name="weight">Weight of <see cref="HeadArmor"></param>
        /// <param name="value">Value of <see cref="HeadArmor"></param>
        /// <param name="currentDurability">Current durability of <see cref="HeadArmor"></param>
        /// <param name="maximumDurability">Maximum durability of <see cref="HeadArmor"></param>
        /// <param name="canSell">Can Sell <see cref="HeadArmor">?</param>
        /// <param name="isSold">Is <see cref="HeadArmor"> Sold?</param>
        internal HeadArmor(string name, string description, int defense, int weight, int value, int currentDurability, int maximumDurability, bool canSell, bool isSold, List<HeroClass> allowedClasses)
        {
            Name = name;
            Description = description;
            Defense = defense;
            Weight = weight;
            Value = value;
            CurrentDurability = currentDurability;
            MaximumDurability = maximumDurability;
            CanSell = canSell;
            IsSold = isSold;
            AllowedClasses = allowedClasses;
        }

        /// <summary>Replaces this instance of <see cref="HeadArmor"> with another instance.</summary>
        /// <param name="other">Instance of <see cref="HeadArmor"> to replace this one</param>
        internal HeadArmor(HeadArmor other) : this(other.Name, other.Description, other.Defense, other.Weight, other.Value, other.CurrentDurability, other.MaximumDurability, other.CanSell, other.IsSold, other.AllowedClasses)
        {
        }

        #endregion Constructors
    }
}