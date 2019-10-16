using Sulimn.Classes.HeroParts;

namespace Sulimn.Classes.Entities
{
    /// <summary>Represents living entities in Sulimn.</summary>
    internal abstract class Character : ICharacter
    {
        #region Modifying Properties

        /// <summary>Name of character</summary>
        public string Name { get; set; }

        /// <summary>Level of character</summary>
        public int Level { get; set; }

        /// <summary>Name of character</summary>
        public int Experience { get; set; }

        /// <summary>Amount of Gold in the inventory.</summary>
        public int Gold { get; set; }

        /// <summary>Attributes of character</summary>
        public Attributes Attributes { get; set; }

        /// <summary>Statistics of character</summary>
        public Statistics Statistics { get; set; }

        /// <summary>Equipment of character</summary>
        public Equipment Equipment { get; set; }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>The experience the Hero has gained this level alongside how much is needed to level up</summary>
        public string ExperienceToString => $"{Experience:N0} / {Level * 100:N0}";

        /// <summary>The experience the Hero has gained this level alongside how much is needed to level up with preceding text</summary>
        public string ExperienceToStringWithText => $"Experience: {ExperienceToString}";

        /// <summary>Amount of Gold in the inventory, with thousands separator.</summary>
        public string GoldToString => Gold.ToString("N0");

        /// <summary>Amount of Gold in the inventory, with thousands separator and preceding text.</summary>
        public string GoldToStringWithText => $"Gold: {GoldToString}";

        /// <summary>Returns the total Strength attribute and bonus produced by the current set of equipment.</summary>
        public int TotalStrength => Attributes.Strength + Equipment.BonusStrength;

        /// <summary>Returns the total Vitality attribute and bonus produced by the current set of equipment.</summary>
        public int TotalVitality => Attributes.Vitality + Equipment.BonusVitality;

        /// <summary>Returns the total Dexterity attribute and bonus produced by the current set of equipment.</summary>
        public int TotalDexterity => Attributes.Dexterity + Equipment.BonusDexterity;

        /// <summary>Returns the total Wisdom attribute and bonus produced by the current set of equipment.</summary>
        public int TotalWisdom => Attributes.Wisdom + Equipment.BonusWisdom;

        #endregion Helper Properties
    }
}