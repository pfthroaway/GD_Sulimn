﻿using Newtonsoft.Json;

namespace Sulimn.Classes.HeroParts
{
    /// <summary>Represents the statistics of an entity.</summary>
    internal class Statistics
    {
        /// <summary>Restores magic to the Hero.</summary>
        /// <param name="restoreAmount">Amount of Magic to be restored.</param>
        /// <returns>String saying magic was restored</returns>
        internal string RestoreMagic(int restoreAmount)
        {
            CurrentMagic += restoreAmount;
            if (CurrentMagic > MaximumMagic)
            {
                CurrentMagic = MaximumMagic;
                return "You restore your magic to its maximum.";
            }
            return $"You restore {restoreAmount:N0} magic.";
        }

        #region Modifying Properties

        [JsonProperty(Order = 1)]
        /// <summary>Amount of current health the Class has.</summary>
        public int CurrentHealth { get; set; }

        [JsonProperty(Order = 2)]
        /// <summary>Amount of maximum health the Class has.</summary>
        public int MaximumHealth { get; set; }

        [JsonProperty(Order = 3)]
        /// <summary>Amount of current magic the Class has.</summary>
        public int CurrentMagic { get; set; }

        [JsonProperty(Order = 4)]
        /// <summary>Amount of maximum magic the Class has.</summary>
        public int MaximumMagic { get; set; }

        #endregion Modifying Properties

        #region Helper Properties

        [JsonIgnore]
        /// <summary>Amount of health the Class has, formatted.</summary>
        public string HealthToString => $"{CurrentHealth:N0} / {MaximumHealth:N0}";

        [JsonIgnore]
        /// <summary>Amount of health the Class has, formatted.</summary>
        public string HealthToStringWithText => $"Health: {HealthToString}";

        [JsonIgnore]
        /// <summary>Amount of magic the Class has, formatted with preceding text.</summary>
        public string MagicToString => $"{CurrentMagic:N0} / {MaximumMagic:N0}";

        [JsonIgnore]
        /// <summary>Amount of magic the Class has, formatted with preceding text.</summary>
        public string MagicToStringWithText => $"Magic: {MagicToString}";

        #endregion Helper Properties

        #region Override Operators

        public static bool Equals(Statistics left, Statistics right)
        {
            if (left is null && right is null) return true;
            if (left is null ^ right is null) return false;
            return left.CurrentHealth == right.CurrentHealth && left.MaximumHealth == right.MaximumHealth && left.CurrentMagic == right.CurrentMagic && left.MaximumMagic == right.MaximumMagic;
        }

        public sealed override bool Equals(object obj) => Equals(this, obj as Statistics);

        public bool Equals(Statistics otherStatistics) => Equals(this, otherStatistics);

        public static bool operator ==(Statistics left, Statistics right) => Equals(left, right);

        public static bool operator !=(Statistics left, Statistics right) => !Equals(left, right);

        public sealed override int GetHashCode() => base.GetHashCode() ^ 17;

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a default instance of Statistics.</summary>
        public Statistics()
        {
        }

        /// <summary>Initializes an instance of Statistics by assigning Properties.</summary>
        /// <param name="currentHealth">Current Health</param>
        /// <param name="maximumHealth">Maximum Health</param>
        /// <param name="currentMagic">Current Magic</param>
        /// <param name="maximumMagic">Maximum Magic</param>
        public Statistics(int currentHealth, int maximumHealth, int currentMagic, int maximumMagic)
        {
            CurrentHealth = currentHealth;
            MaximumHealth = maximumHealth;
            CurrentMagic = currentMagic;
            MaximumMagic = maximumMagic;
        }

        /// <summary>Replaces this instance of Statistics with another instance.</summary>
        /// <param name="other">Instance to replace this instance</param>
        public Statistics(Statistics other) : this(other.CurrentHealth, other.MaximumHealth, other.CurrentMagic,
            other.MaximumMagic)
        {
        }

        #endregion Constructors
    }
}