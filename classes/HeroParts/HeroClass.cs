using Newtonsoft.Json;
using Sulimn.Classes.Entities;
using System;

namespace Sulimn.Classes.HeroParts
{
    /// <summary>Represents the <see cref="HeroClass"/> of a <see cref="Hero"/>.</summary>
    public class HeroClass : IEquatable<HeroClass>
    {
        private int _vitality, _wisdom;

        #region Modifying Properties

        /// <summary>Name of the <see cref="HeroClass"/>.</summary>
        [JsonProperty(Order = 1)]
        public string Name { get; set; }

        /// <summary>Description of the <see cref="HeroClass"/>.</summary>
        [JsonProperty(Order = 2)]
        public string Description { get; set; }

        /// <summary>Maximum number of skill points a <see cref="HeroClass"/> can have when initially being assigned.</summary>
        [JsonProperty(Order = 3)]
        public int SkillPoints { get; set; }

        /// <summary>Amount of Strength the <see cref="HeroClass"/> has by default.</summary>
        [JsonProperty(Order = 4)]
        public int Strength { get; set; }

        /// <summary>Amount of Vitality the <see cref="HeroClass"/> has by default.</summary>
        [JsonProperty(Order = 5)]
        public int Vitality
        {
            get => _vitality;
            set
            {
                _vitality = value;
                CurrentHealth = Vitality * 5;
                MaximumHealth = Vitality * 5;
            }
        }

        /// <summary>Amount of Dexterity the <see cref="HeroClass"/> has by default.</summary>
        [JsonProperty(Order = 6)]
        public int Dexterity { get; set; }

        /// <summary>Amount of Wisdom the <see cref="HeroClass"/> has by default.</summary>
        [JsonProperty(Order = 7)]
        public int Wisdom
        {
            get => _wisdom;
            set
            {
                _wisdom = value;
                CurrentMagic = Wisdom * 5;
                MaximumMagic = Wisdom * 5;
            }
        }

        /// <summary>Amount of current health the <see cref="HeroClass"/> has.</summary>
        [JsonIgnore]
        public int CurrentHealth { get; set; }

        /// <summary>Amount of maximum health the <see cref="HeroClass"/> has.</summary>
        [JsonIgnore]
        public int MaximumHealth { get; set; }

        /// <summary>Amount of current magic the <see cref="HeroClass"/> has.</summary>
        [JsonIgnore]
        public int CurrentMagic { get; set; }

        /// <summary>Amount of maximum magic the <see cref="HeroClass"/> has.</summary>
        [JsonIgnore]
        public int MaximumMagic { get; set; }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>Maximum number of skill points a <see cref="HeroClass"/> can have when initially being assigned, with thousands separator.</summary>
        [JsonIgnore]
        public string SkillPointsToString => SkillPoints != 1 ? $"{SkillPoints:N0} Skill Points Available" : $"{SkillPoints:N0} Skill Point Available";

        /// <summary>Amount of health the <see cref="HeroClass"/> has, formatted.</summary>
        [JsonIgnore]
        public string HealthToString => $"{CurrentHealth:N0} / {MaximumHealth:N0}";

        /// <summary>Amount of health the <see cref="HeroClass"/> has, formatted with preceding text.</summary>
        [JsonIgnore]
        public string HealthToStringWithText => $"Health: {HealthToString}";

        /// <summary>Amount of magic the <see cref="HeroClass"/> has, formatted.</summary>
        [JsonIgnore]
        public string MagicToString => $"{CurrentMagic:N0} / {MaximumMagic:N0}";

        /// <summary>Amount of magic the <see cref="HeroClass"/> has, formatted with preceding text.</summary>
        [JsonIgnore]
        public string MagicToStringWithText => $"Magic: {MagicToString}";

        #endregion Helper Properties

        #region Override Operators

        public static bool Equals(HeroClass left, HeroClass right)
        {
            if (left is null && right is null) return true;
            if (left is null ^ right is null) return false;
            return string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(left.Description, right.Description, StringComparison.OrdinalIgnoreCase)
                   && left.SkillPoints == right.SkillPoints
                   && left.Strength == right.Strength
                   && left.Vitality == right.Vitality
                   && left.Dexterity == right.Dexterity
                   && left.Wisdom == right.Wisdom
                   && left.CurrentHealth == right.CurrentHealth
                   && left.MaximumHealth == right.MaximumHealth
                   && left.CurrentMagic == right.CurrentMagic
                   && left.MaximumMagic == right.MaximumMagic;
        }

        public sealed override bool Equals(object obj) => Equals(this, obj as HeroClass);

        public bool Equals(HeroClass other) => Equals(this, other);

        public static bool operator ==(HeroClass left, HeroClass right) => Equals(left, right);

        public static bool operator !=(HeroClass left, HeroClass right) => !Equals(left, right);

        public sealed override int GetHashCode() => base.GetHashCode() ^ 17;

        public sealed override string ToString() => Name;

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a default instance of <see cref="HeroClass"/>.</summary>
        internal HeroClass()
        {
        }

        /// <summary>Initializes an instance of <see cref="HeroClass"/> by assigning Properties.</summary>
        /// <param name="name">Name of <see cref="HeroClass"/></param>
        /// <param name="description">Description of <see cref="HeroClass"/></param>
        /// <param name="skillPoints">Skill Points</param>
        /// <param name="strength">Strength</param>
        /// <param name="vitality">Vitality</param>
        /// <param name="dexterity">Dexterity</param>
        /// <param name="wisdom">Wisdom</param>
        internal HeroClass(string name, string description, int skillPoints, int strength, int vitality, int dexterity,
        int wisdom)
        {
            Name = name;
            Description = description;
            SkillPoints = skillPoints;
            Strength = strength;
            Vitality = vitality;
            Dexterity = dexterity;
            Wisdom = wisdom;
        }

        /// <summary>Replaces this instance of <see cref="HeroClass"/> with another instance.</summary>
        /// <param name="other">Instance of <see cref="HeroClass"/> to replace this instance</param>
        internal HeroClass(HeroClass other) : this(other.Name, other.Description, other.SkillPoints, other.Strength,
            other.Vitality, other.Dexterity, other.Wisdom)
        {
        }

        #endregion Constructors
    }
}