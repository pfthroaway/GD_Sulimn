using Newtonsoft.Json;
using Sulimn.Classes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sulimn.Classes.HeroParts
{
    /// <summary>Represents a Spell a Hero can cast.</summary>
    public class Spell : IEquatable<Spell>
    {
        #region Modifying Properties

        /// <summary>Name of the Spell.</summary>
        public string Name { get; set; }

        /// <summary>Description of the Spell.</summary>
        public string Description { get; set; }

        /// <summary>Type of the Spell.</summary>
        public SpellType Type { get; set; }

        [JsonIgnore]
        /// <summary><see cref="HeroClass"/>es allowed to use the Spell.</summary>
        public List<HeroClass> AllowedClasses { get; set; }

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

        /// <summary>Required Level of the Spell.</summary>
        public int MinimumLevel { get; set; }

        /// <summary>Magic cost of the Spell.</summary>
        public int MagicCost { get; set; }

        /// <summary>Amount of the Spell.</summary>
        public int Amount { get; set; }

        #endregion Modifying Properties

        #region Helper Properties

        [JsonIgnore]
        /// <summary><see cref="HeroClass"/>es allowed to use the Spell, formatted.</summary>
        public string AllowedClassesToString => AllowedClasses?.Count > 0 ? string.Join(",", AllowedClasses) : "";

        [JsonIgnore]
        /// <summary><see cref="HeroClass"/>es allowed to use the Spell, formatted, with preceding text.</summary>
        public string AllowedClassesToStringWithText => AllowedClasses?.Count > 0 ? $"Allowed Classes: {string.Join(",", AllowedClasses)}" : "";

        [JsonIgnore]
        /// <summary>Type of the Spell, in string format.</summary>
        public string TypeToString => !string.IsNullOrWhiteSpace(Name) ? Type.ToString() : "";

        [JsonIgnore]
        /// <summary>Type and amount of the Spell.</summary>
        public string TypeAmount => Amount > 0 ? $"{Type}: {Amount}" : "";

        [JsonIgnore]
        /// <summary>Magic cost of the Spell, with preceding text.</summary>
        public string MagicCostToString => MagicCost > 0 ? $"Magic Cost: {MagicCost:N0}" : "";

        [JsonIgnore]
        /// <summary>Required Level of the Spell, with preceding text.</summary>
        public string RequiredLevelToString => !string.IsNullOrWhiteSpace(Name) ? $"Required Level: {MinimumLevel}" : "";

        [JsonIgnore]
        /// <summary>Value of the Spell.</summary>
        public int Value => MinimumLevel * 200;

        [JsonIgnore]
        /// <summary>Value of the Spell, with preceding text.</summary>
        public string ValueToString => Value.ToString("N0");

        [JsonIgnore]
        /// <summary>Value of the Spell, with thousands separator and preceding text.</summary>
        public string ValueToStringWithText => !string.IsNullOrWhiteSpace(Name) ? $"Value: {ValueToString}" : "";

        #endregion Helper Properties

        #region Override Operators

        public static bool Equals(Spell left, Spell right)
        {
            if (left is null && right is null) return true;
            if (left is null ^ right is null) return false;
            return string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase)
                   && left.Type == right.Type
                   && string.Equals(left.Description, right.Description, StringComparison.OrdinalIgnoreCase)
                   && !left.AllowedClasses.Except(right.AllowedClasses).Any()
                   && left.MinimumLevel == right.MinimumLevel
                   && left.MagicCost == right.MagicCost
                   && left.Amount == right.Amount;
        }

        public sealed override bool Equals(object obj) => Equals(this, obj as Spell);

        public bool Equals(Spell other) => Equals(this, other);

        public static bool operator ==(Spell left, Spell right) => Equals(left, right);

        public static bool operator !=(Spell left, Spell right) => !Equals(left, right);

        public sealed override int GetHashCode() => base.GetHashCode() ^ 17;

        public sealed override string ToString() => Name;

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a default instance of Spell.</summary>
        internal Spell()
        {
        }

        /// <summary>Initializes an instance of <see cref="Spell"/> by assigning Properties.</summary>
        /// <param name="name">Name of <see cref="Spell"/></param>
        /// <param name="spellType">Type of <see cref="Spell"/></param>
        /// <param name="description">Description of <see cref="Spell"/></param>
        /// <param name="allowedClasses"><see cref="HeroClass"/>es allowed to learn the <see cref="<see cref="Spell"/>"/></param>
        /// <param name="minimumLevel">Required Level to learn <see cref="Spell"/></param>
        /// <param name="magicCost">Magic cost of <see cref="Spell"/></param>
        /// <param name="amount">Amount of <see cref="Spell"/></param>
        internal Spell(string name, SpellType spellType, string description, List<HeroClass> allowedClasses, int minimumLevel,
        int magicCost, int amount)
        {
            Name = name;
            Type = spellType;
            Description = description;
            AllowedClasses = allowedClasses;
            MinimumLevel = minimumLevel;
            MagicCost = magicCost;
            Amount = amount;
        }

        #endregion Constructors
    }
}