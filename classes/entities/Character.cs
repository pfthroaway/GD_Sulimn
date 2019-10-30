using Newtonsoft.Json;
using Sulimn.Classes.HeroParts;

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
        public Attributes Attributes { get; set; }

        /// <summary>Statistics of character</summary>
        [JsonProperty(Order = 5)]
        public Statistics Statistics { get; set; }

        /// <summary>Equipment of character</summary>
        [JsonProperty(Order = 6)]
        public Equipment Equipment { get; set; }

        #endregion Modifying Properties

        #region Helper Properties

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
    }
}