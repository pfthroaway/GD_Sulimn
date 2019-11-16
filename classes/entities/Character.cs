using Newtonsoft.Json;
using Sulimn.Classes.HeroParts;
using Sulimn.Classes.Items;
using System.Collections.Generic;
using System.Linq;

namespace Sulimn.Classes.Entities
{
    /// <summary>Represents living entities in Sulimn.</summary>
    internal abstract class Character : ICharacter
    {
        #region Modifying Properties

        /// <summary>Name of character</summary>
        [JsonProperty(Order = -5)]
        public string Name { get; set; }

        /// <summary>Level of character</summary>
        [JsonProperty(Order = 1)]
        public int Level { get; set; }

        /// <summary>Name of character</summary>
        [JsonProperty(Order = 2)]
        public int Experience { get; set; }

        /// <summary>Amount of Gold in the inventory.</summary>
        [JsonProperty(Order = 3)]
        public int Gold { get; set; }

        /// <summary>Attributes of character</summary>
        [JsonProperty(Order = 4)]
        public Attributes Attributes { get; set; } = new Attributes();

        /// <summary>Statistics of character</summary>
        [JsonProperty(Order = 5)]
        public Statistics Statistics { get; set; } = new Statistics();

        /// <summary>Equipment of character</summary>
        [JsonProperty(Order = 6)]
        public Equipment Equipment { get; set; } = new Equipment();

        /// <summary>The list of Spells the Hero currently knows</summary>
        [JsonProperty(Order = 8)]
        public Spellbook Spellbook { get; set; } = new Spellbook();

        /// <summary>List of Items in the inventory.</summary>
        [JsonProperty(Order = 10)]
        public List<Item> Inventory { get; set; } = new List<Item>();

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>Returns the Enemy's level with preceding text.</summary>
        [JsonIgnore]
        public string LevelToString => $"Level {Level}";

        /// <summary>The experience the Hero has gained this level alongside how much is needed to level up</summary>
        [JsonIgnore]
        public string ExperienceToString => $"{Experience:N0} / {Level * 100:N0}";

        /// <summary>The experience the Hero has gained this level alongside how much is needed to level up with preceding text</summary>
        [JsonIgnore]
        public string ExperienceToStringWithText => $"Experience: {ExperienceToString}";

        /// <summary>Amount of Gold in the inventory, with thousands separator.</summary>
        [JsonIgnore]
        public string GoldToString => Gold.ToString("N0");

        /// <summary>Amount of Gold in the inventory, with thousands separator and preceding text.</summary>
        [JsonIgnore]
        public string GoldToStringWithText => $"Gold: {GoldToString}";

        /// <summary>List of Items in the inventory, formatted.</summary>
        [JsonIgnore]
        public string InventoryToString => string.Join(",", Inventory);

        /// <summary>Combined weight of all Items in a Hero's Inventory.</summary>
        [JsonIgnore]
        public int CarryingWeight => Inventory.Count > 0 ? Inventory.Sum(itm => itm.Weight) : 0;

        /// <summary>Combined weight of all Items in a Hero's Inventory and all the Equipment currently equipped.</summary>
        [JsonIgnore]
        public int TotalWeight => CarryingWeight + Equipment.TotalWeight;

        /// <summary>Maximum weight a Hero can carry.</summary>
        [JsonIgnore]
        public int MaximumWeight => TotalStrength * 10;

        /// <summary>Is the Hero carrying more than they should be able to?</summary>
        [JsonIgnore]
        public bool Overweight => TotalWeight > MaximumWeight;

        /// <summary>Ratio of Total Strength to Total Weight.</summary>
        [JsonIgnore]
        public decimal StrengthWeightRatio => TotalStrength * 10m / TotalWeight;

        /// <summary>Total Strength value including from Attributes and Equipment.</summary>
        [JsonIgnore]
        public int TotalStrength => Attributes.Strength + Equipment.BonusStrength;

        /// <summary>Total Vitality value including from Attributes and Equipment.</summary>
        [JsonIgnore]
        public int TotalVitality => Attributes.Vitality + Equipment.BonusVitality;

        /// <summary>Total Dexterity value including from Attributes and Equipment.</summary>
        [JsonIgnore]
        public int TotalDexterity => Attributes.Dexterity + Equipment.BonusDexterity;

        /// <summary>Total Wisdom value including from Attributes and Equipment.</summary>
        [JsonIgnore]
        public int TotalWisdom => Attributes.Wisdom + Equipment.BonusWisdom;

        #endregion Helper Properties

        #region Inventory Management

        /// <summary>Adds an Item to the inventory.</summary>
        /// <param name="item">Item to be removed</param>
        internal void AddItem(Item item)
        {
            Inventory.Add(item);
            Inventory = Inventory.OrderBy(itm => itm.Name).ToList();
        }

        /// <summary>Removes an Item from the inventory.</summary>
        /// <param name="item">Item to be removed</param>
        internal void RemoveItem(Item item) => Inventory.Remove(item);

        #endregion Inventory Management
    }
}