using Newtonsoft.Json;
using Sulimn.Classes.HeroParts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sulimn.Classes.Items
{
    internal class Item : IItem
    {
        //TODO Implement durability and other new features, maybe weapon/armor smiths.

        #region Modifying Properties

        [JsonProperty(Order = -6)]
        /// <summary>Name of the <see cref="Item"/></summary>
        public string Name { get; set; }

        [JsonProperty(Order = -5)]
        /// <summary>Description of the <see cref="Item"/></summary>
        public string Description { get; set; }

        [JsonProperty(Order = -3)]
        /// <summary>How much the <see cref="Item"/> weighs.</summary>
        public int Weight { get; set; }

        [JsonProperty(Order = -2)]
        /// <summary>How much the <see cref="Item"/> is worth</summary>
        public int Value { get; set; }

        [JsonProperty(Order = -1)]
        /// <summary>The current durability of an <see cref="Item"/>.</summary>
        public int CurrentDurability { get; set; }

        [JsonProperty(Order = -0)]
        /// <summary>The maximum durability of an <see cref="Item"/>.</summary>
        public int MaximumDurability { get; set; }

        [JsonIgnore]
        /// <summary>The minimum level a <see cref="Hero"/> is required to be to use the <see cref="Item"/>.</summary>
        public int MinimumLevel { get; set; }

        [JsonProperty(Order = 3)]
        /// <summary>Can the <see cref="Item"/> be sold to a shop?</summary>
        public bool CanSell { get; set; }

        [JsonProperty(Order = 4)]
        /// <summary>Can the <see cref="Item"/> be sold in a shop?</summary>
        public bool IsSold { get; set; }

        [JsonIgnore]
        /// <summary>Classes permitted to use this <see cref="Item"/>.</summary>
        public List<HeroClass> AllowedClasses { get; set; }

        [JsonProperty(Order = 5)]
        /// <summary><see cref="HeroClass"/>es allowed to use the Spell, set up to import from JSON.</summary>
        public string AllowedClassesJson
        {
            get => AllowedClasses?.Count > 0 ? string.Join(",", AllowedClasses) : "";
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    AllowedClasses = new List<HeroClass>();
                    AllowedClasses.AddRange(from string heroClass in value.Split(',')
                                            select GameState.AllClasses.Find(cls => cls.Name == heroClass));
                }
            }
        }

        #endregion Modifying Properties

        #region Helper Properties

        [JsonIgnore]
        /// <summary>The current durability of an <see cref="Item"/>, with thousands separators.</summary>
        public string CurrentDurabilityToString => CurrentDurability.ToString("N0");

        [JsonIgnore]
        /// <summary>The maximum durability of an <see cref="Item"/>, with thousands separators.</summary>
        public string MaximumDurabilityToString => MaximumDurability.ToString("N0");

        [JsonIgnore]
        /// <summary>The durability of an <see cref="Item"/>, formatted.</summary>
        public string Durability => $"{CurrentDurabilityToString} / {MaximumDurabilityToString}";

        [JsonIgnore]
        /// <summary>The value of the <see cref="Item"/> with thousands separators.</summary>
        public string ValueToString => Value.ToString("N0");

        [JsonIgnore]
        /// <summary>The value of the <see cref="Item"/> with thousands separators and preceding text.</summary>
        public string ValueToStringWithText => !string.IsNullOrWhiteSpace(Name) ? $"Value: {ValueToString}" : "";

        [JsonIgnore]
        /// <summary>The value of the Item.</summary>
        public int SellValue => Value / 2;

        [JsonIgnore]
        /// <summary>The sell value of the <see cref="Item"/> with thousands separators.</summary>
        public string SellValueToString => SellValue.ToString("N0");

        [JsonIgnore]
        /// <summary>The sell value of the <see cref="Item"/> with thousands separators with preceding text.</summary>
        public string SellValueToStringWithText => !string.IsNullOrWhiteSpace(Name) ? $"Sell Value: {SellValueToString}" : "";

        [JsonIgnore]
        /// <summary>Returns text relating to the sellability of the <see cref="Item"/>.</summary>
        public string CanSellToString => !string.IsNullOrWhiteSpace(Name) ? (CanSell ? "Sellable" : "Not Sellable") : "";

        [JsonIgnore]
        public string AllowedClassesToString => String.Join(",", AllowedClasses);

        #endregion Helper Properties

        #region Override Operators

        public static bool Equals(Item left, Item right)
        {
            if (left is null && right is null) return true;
            if (left is null ^ right is null) return false;
            return string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase) && string.Equals(left.Description, right.Description, StringComparison.OrdinalIgnoreCase) && left.Weight == right.Weight && left.Value == right.Value && left.CanSell == right.CanSell && left.IsSold == right.IsSold;
        }

        public override bool Equals(object obj) => Equals(this, obj as Item);

        public bool Equals(Item otherItem) => Equals(this, otherItem);

        public static bool operator ==(Item left, Item right) => Equals(left, right);

        public static bool operator !=(Item left, Item right) => !Equals(left, right);

        public override int GetHashCode() => base.GetHashCode() ^ 17;

        public override string ToString() => Name;

        #endregion Override Operators
    }
}