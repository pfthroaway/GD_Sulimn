using Newtonsoft.Json;
using Sulimn.Classes.HeroParts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sulimn.Classes.Items
{
    internal class Item
    {
        //TODO Implement durability and other new features, maybe weapon/armor smiths.

        #region Modifying Properties

        [JsonProperty(Order = 1)]
        /// <summary>Name of the <see cref="Item"/>.</summary>
        public string Name { get; set; }

        [JsonProperty(Order = 2)]
        /// <summary>Description of the <see cref="Item"/>.</summary>
        public string Description { get; set; }

        [JsonProperty(Order = 3)]
        /// <summary>Type of the <see cref="Item"/>.</summary>
        public ItemType Type { get; set; }

        [JsonProperty(Order = 4)]
        /// <summary>Amount of  the <see cref="Item"/> inflicts.</summary>
        public int Damage { get; set; }

        [JsonProperty(Order = 5)]
        /// <summary>Amount of damage the <see cref="Item"/> can defend against.</summary>
        public int Defense { get; set; }

        [JsonProperty(Order = 6)]
        /// <summary>Amount the <see cref="Item"/> weighs.</summary>
        public int Weight { get; set; }

        [JsonProperty(Order = 7)]
        /// <summary>Amount the <see cref="Item"/> is worth.</summary>
        public int Value { get; set; }

        [JsonProperty(Order = 8)]
        /// <summary>The current durability of an <see cref="Item"/>.</summary>
        public int CurrentDurability { get; set; }

        [JsonProperty(Order = 9)]
        /// <summary>The maximum durability of an <see cref="Item"/>.</summary>
        public int MaximumDurability { get; set; }

        [JsonProperty(Order = 10)]
        /// <summary>Amount of bonus Strength the <see cref="Item"/> grants.</summary>
        public int Strength { get; set; }

        [JsonProperty(Order = 11)]
        /// <summary>Amount of bonus Vitality the <see cref="Item"/> grants.</summary>
        public int Vitality { get; set; }

        [JsonProperty(Order = 12)]
        /// <summary>Amount of bonus Dexterity the <see cref="Item"/> grants.</summary>
        public int Dexterity { get; set; }

        [JsonProperty(Order = 13)]
        /// <summary>Amount of bonus Wisdom the <see cref="Item"/> grants.</summary>
        public int Wisdom { get; set; }

        [JsonProperty(Order = 14)]
        /// <summary>Amount of health this <see cref="Item"/> restores.</summary>
        public int RestoreHealth { get; set; }

        [JsonProperty(Order = 15)]
        /// <summary>Amount of health this <see cref="Item"/> restores.</summary>
        public int RestoreMagic { get; set; }

        [JsonProperty(Order = 16)]
        /// <summary>Does this <see cref="Item"/> cure?</summary>
        public bool Cures { get; set; }

        [JsonProperty(Order = 17)]
        /// <summary>The minimum level a <see cref="Hero"/> is required to be to use the <see cref="Item"/>.</summary>
        public int MinimumLevel { get; set; }

        [JsonProperty(Order = 18)]
        /// <summary>Can the <see cref="Item"/> be sold to a shop?</summary>
        public bool CanSell { get; set; }

        [JsonProperty(Order = 19)]
        /// <summary>Is the <see cref="Item"/> sold in a shop?</summary>
        public bool IsSold { get; set; }

        [JsonIgnore]
        /// <summary><see cref="HeroClass"/>es permitted to use this <see cref="Item"/>.</summary>
        public List<HeroClass> AllowedClasses { get; set; } = new List<HeroClass>();

        [JsonProperty(Order = 20)]
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
        /// <summary>Damage the weapon inflicts, formatted.</summary>
        public string DamageToString => Damage.ToString("N0");

        [JsonIgnore]
        /// <summary>Damage the weapon inflicts, formatted, with preceding text.</summary>
        public string DamageToStringWithText => Damage > 0 ? $"Damage: {DamageToString}" : "";

        [JsonIgnore]
        /// <summary>Type of Weapon to string.</summary>
        public string WeaponTypeToString => Type == ItemType.MeleeWeapon ? "Melee" : Type == ItemType.MeleeWeapon ? "Ranged" : "";

        [JsonIgnore]
        /// <summary>Returns the defense with a comma separating thousands.</summary>
        public string DefenseToString => Defense.ToString("N0");

        [JsonIgnore]
        /// <summary>Returns the defense with a comma separating thousands and preceding text.</summary>
        public string DefenseToStringWithText => Defense > 0 ? $"Defense: {DefenseToString}" : "";

        [JsonIgnore]
        /// <summary>Returns text relating to the amount of Health restored by the <see cref="Consumable"/>.</summary>
        public string RestoreHealthToString => RestoreHealth > 0 ? $"Restores {RestoreHealth:N0} Health." : "";

        [JsonIgnore]
        /// <summary>Returns text relating to the amount of Magic restored by the <see cref="Consumable"/>.</summary>
        public string RestoreMagicToString => RestoreMagic > 0 ? $"Restores {RestoreMagic:N0} Magic." : "";

        [JsonIgnore]
        /// <summary>Returns text regarding if the <see cref="Consumable"/> can heal the user.</summary>
        public string CuresToString => Cures ? $"Cures ailments." : "";

        [JsonIgnore]
        /// <summary>Returns text regarding all effects this <see cref="Consumable"/> will induce.</summary>
        public string EffectsToString
        {
            get
            {
                string effects = "";
                if (RestoreHealth > 0)
                {
                    effects += RestoreHealthToString;
                }
                if (RestoreMagic > 0)
                {
                    if (effects.Length > 0)
                        effects += "\n";
                    effects += RestoreMagicToString;
                }
                if (Cures)
                {
                    if (effects.Length > 0)
                        effects += "\n";
                    effects += CuresToString;
                }

                return effects;
            }
        }

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
        /// <summary>Returns the Strength and preceding text.</summary>
        public string StrengthToString => Strength > 0 ? $"Strength: {Strength}" : "";

        [JsonIgnore]
        /// <summary>Returns the Vitality and preceding text.</summary>
        public string VitalityToString => Vitality > 0 ? $"Vitality: {Vitality}" : "";

        [JsonIgnore]
        /// <summary>Returns the Dexterity and preceding text.</summary>
        public string DexterityToString => Dexterity > 0 ? $"Dexterity: {Dexterity}" : "";

        [JsonIgnore]
        /// <summary>Returns the Wisdom and preceding text.</summary>
        public string WisdomToString => Wisdom > 0 ? $"Wisdom: {Wisdom}" : "";

        [JsonIgnore]
        /// <summary>Returns all bonuses in string format.</summary>
        public string BonusToString
        {
            get
            {
                string[] bonuses =
                {
                    DamageToStringWithText, DefenseToStringWithText, StrengthToString, VitalityToString,
                    DexterityToString, WisdomToString
                };

                return string.Join(", ", bonuses.Where(bonus => bonus.Length > 0));
            }
        }

        [JsonIgnore]
        public string AllowedClassesToString => String.Join(",", AllowedClasses);

        #endregion Helper Properties

        #region Override Operators

        public static bool Equals(Item left, Item right)
        {
            if (left is null && right is null) return true;
            if (left is null ^ right is null) return false;
            return string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase)
                && string.Equals(left.Description, right.Description, StringComparison.OrdinalIgnoreCase)
                && left.Type == right.Type
                && left.Damage == right.Damage
                && left.Defense == right.Defense
                && left.Weight == right.Weight
                && left.Value == right.Value
                && left.CurrentDurability == right.CurrentDurability
                && left.MaximumDurability == right.MaximumDurability
                && left.Strength == right.Strength
                && left.Vitality == right.Vitality
                && left.Dexterity == right.Dexterity
                && left.Wisdom == right.Wisdom
                && left.RestoreHealth == right.RestoreHealth
                && left.RestoreMagic == right.RestoreMagic
                && left.Cures == right.Cures
                && left.MinimumLevel == right.MinimumLevel
                && left.CanSell == right.CanSell
                && left.IsSold == right.IsSold
                && !left.AllowedClasses.Except(right.AllowedClasses).Any();
        }

        public override bool Equals(object obj) => Equals(this, obj as Item);

        public bool Equals(Item otherItem) => Equals(this, otherItem);

        public static bool operator ==(Item left, Item right) => Equals(left, right);

        public static bool operator !=(Item left, Item right) => !Equals(left, right);

        public override int GetHashCode() => base.GetHashCode() ^ 17;

        public override string ToString() => Name;

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a default instance of <see cref="Item"/>.</summary>
        public Item()
        {
        }

        /// <summary>Hello</summary>
        /// <param name="name">Name of the <see cref="Item"/></param>
        /// <param name="description">Description of the <see cref="Item"/></param>
        /// <param name="itemType">Type of the <see cref="Item"/></param>
        /// <param name="damage">Amount of damage the <see cref="Item"/> inflicts</param>
        /// <param name="defense">Amount of damage the <see cref="Item"/> can defend against</param>
        /// <param name="weight">Amount the <see cref="Item"/> weighs</param>
        /// <param name="value">Amount the <see cref="Item"/> is worth</param>
        /// <param name="currentDurability">The current durability of an <see cref="Item"/></param>
        /// <param name="maximumDurability">The maximum durability of an <see cref="Item"/></param>
        /// <param name="strength">Amount of bonus Strength the <see cref="Item"/> grants</param>
        /// <param name="vitality">Amount of bonus Vitality the <see cref="Item"/> grants</param>
        /// <param name="dexterity">Amount of bonus Dexterity the <see cref="Item"/> grants</param>
        /// <param name="wisdom">Amount of bonus Wisdom the <see cref="Item"/> grants</param>
        /// <param name="restoreHealth">Amount of health this <see cref="Item"/> restores</param>
        /// <param name="restoreMagic">Amount of magic this <see cref="Item"/> restores</param>
        /// <param name="cures">Does this <see cref="Item"/> cure?</param>
        /// <param name="minimumLevel">The minimum level a <see cref="Hero"/> is required to be to use the <see cref="Item"/></param>
        /// <param name="canSell">Can the <see cref="Item"/> be sold to a shop?</param>
        /// <param name="isSold">Is the <see cref="Item"/> sold in a shop?</param>
        /// <param name="allowedClasses"><see cref="HeroClass"/>es permitted to use this <see cref="Item"/></param>
        internal Item(string name, string description, ItemType itemType, int damage, int defense, int weight, int value, int currentDurability, int maximumDurability, int strength, int vitality, int dexterity, int wisdom, int restoreHealth, int restoreMagic, bool cures, int minimumLevel, bool canSell, bool isSold, List<HeroClass> allowedClasses)
        {
            Name = name;
            Description = description;
            Type = itemType;
            Damage = damage;
            Defense = defense;
            Weight = weight;
            Value = value;
            CurrentDurability = currentDurability;
            MaximumDurability = maximumDurability;
            Strength = strength;
            Vitality = vitality;
            Dexterity = dexterity;
            Wisdom = wisdom;
            RestoreHealth = restoreHealth;
            RestoreMagic = restoreMagic;
            Cures = cures;
            MinimumLevel = minimumLevel;
            CanSell = canSell;
            IsSold = isSold;
            AllowedClasses = new List<HeroClass>(allowedClasses);
        }

        /// <summary>Replaces this instance of <see cref="Item"/> with another instance.</summary>
        /// <param name="other">YP</param>
        public Item(Item other) : this(other.Name, other.Description, other.Type, other.Damage, other.Defense, other.Weight, other.Value, other.CurrentDurability, other.MaximumDurability, other.Strength, other.Vitality, other.Dexterity, other.Wisdom, other.RestoreHealth, other.RestoreMagic, other.Cures, other.MinimumLevel, other.CanSell, other.IsSold, new List<HeroClass>(other.AllowedClasses))
        {
        }

        #endregion Constructors
    }
}