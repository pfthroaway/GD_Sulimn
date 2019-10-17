using Newtonsoft.Json;
using Sulimn.Classes.HeroParts;

namespace Sulimn.Classes.Entities
{
    /// <summary>Represents living entities in Sulimn.</summary>
    internal abstract class Character : ICharacter
    {
        #region Modifying Properties

        [JsonProperty(Order = -5)]
        /// <summary>Name of character</summary>
        public string Name { get; set; }

        [JsonProperty(Order = 1)]
        /// <summary>Level of character</summary>
        public int Level { get; set; }

        [JsonProperty(Order = 2)]
        /// <summary>Name of character</summary>
        public int Experience { get; set; }

        [JsonProperty(Order = 3)]
        /// <summary>Amount of Gold in the inventory.</summary>
        public int Gold { get; set; }

        [JsonProperty(Order = 4)]
        /// <summary>Attributes of character</summary>
        public Attributes Attributes { get; set; }

        [JsonProperty(Order = 5)]
        /// <summary>Statistics of character</summary>
        public Statistics Statistics { get; set; }

        [JsonProperty(Order = 6)]
        /// <summary>Equipment of character</summary>
        public Equipment Equipment { get; set; }

        #endregion Modifying Properties

        #region Helper Properties

        [JsonIgnore]
        /// <summary>The experience the Hero has gained this level alongside how much is needed to level up</summary>
        public string ExperienceToString => $"{Experience:N0} / {Level * 100:N0}";

        [JsonIgnore]
        /// <summary>The experience the Hero has gained this level alongside how much is needed to level up with preceding text</summary>
        public string ExperienceToStringWithText => $"Experience: {ExperienceToString}";

        [JsonIgnore]
        /// <summary>Amount of Gold in the inventory, with thousands separator.</summary>
        public string GoldToString => Gold.ToString("N0");

        [JsonIgnore]
        /// <summary>Amount of Gold in the inventory, with thousands separator and preceding text.</summary>
        public string GoldToStringWithText => $"Gold: {GoldToString}";

        [JsonIgnore]
        public int TotalStrength => Attributes.Strength + Equipment.BonusStrength;

        [JsonIgnore]
        public int TotalVitality => Attributes.Vitality + Equipment.BonusVitality;

        [JsonIgnore]
        public int TotalDexterity => Attributes.Dexterity + Equipment.BonusDexterity;

        [JsonIgnore]
        public int TotalWisdom => Attributes.Wisdom + Equipment.BonusWisdom;

        #endregion Helper Properties
    }
}