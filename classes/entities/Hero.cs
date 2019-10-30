using Newtonsoft.Json;
using Sulimn.Classes.Enums;
using Sulimn.Classes.HeroParts;
using Sulimn.Classes.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sulimn.Classes.Entities
{
    /// <summary>Represents a Hero from Sulimn.</summary>
    internal class Hero : Character
    {
        #region Modifying Properties

        /// <summary>The hashed password of the Hero</summary>
        [JsonProperty(Order = -4)]
        public string Password { get; set; }

        /// <summary>The HeroClass of the Hero, set up to import from JSON.</summary>
        [JsonIgnore]
        public HeroClass Class { get; set; }

        /// <summary>The HeroClass of the Hero</summary>
        [JsonProperty(Order = -3)]
        public string ClassString
        {
            get => Class.Name;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    Class = GameState.AllClasses.Find(o => o.Name == value);
            }
        }

        /// <summary>The amount of available skill points the Hero has</summary>
        [JsonProperty(Order = -1)]
        public int SkillPoints { get; set; }

        /// <summary>The progress the Hero has made.</summary>
        [JsonProperty(Order = 7)]
        public Progression Progression { get; set; }

        /// <summary>The list of Spells the Hero currently knows</summary>
        [JsonProperty(Order = 8)]
        public Spellbook Spellbook { get; set; }

        /// <summary>The <see cref="Hero"/>'s <see cref="HeroParts.Bank"/>. </summary>
        [JsonProperty(Order = 9)]
        public Bank Bank { get; set; }

        /// <summary>Will the player be deleted on death?</summary>
        [JsonProperty(Order = -2)]
        public bool Hardcore { get; set; }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>List of Items in the inventory.</summary>
        [JsonProperty(Order = 10)]
        public List<Item> Inventory { get; set; }

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

        /// <summary>Will the player be deleted on death?</summary>
        [JsonIgnore]
        public string HardcoreToString => Hardcore ? "Hardcore" : "Softcore";

        /// <summary>The level and class of the Hero</summary>
        [JsonIgnore]
        public string LevelAndClassToString => $"Level {Level} {Class.Name}";

        /// <summary>The amount of skill points the Hero has available to spend, formatted.</summary>
        [JsonIgnore]
        public string SkillPointsToString => SkillPoints != 1 ? $"{SkillPoints:N0} Skill Points Available" : $"{SkillPoints:N0} Skill Point Available";

        #endregion Helper Properties

        /// <summary>Updates the Hero's Statistics.</summary>
        internal void UpdateStatistics()
        {
            if (Statistics.MaximumHealth != (TotalVitality + Level - 1) * 5)
            {
                int diff = ((TotalVitality + Level - 1) * 5) - Statistics.MaximumHealth;

                Statistics.CurrentHealth += diff;
                Statistics.MaximumHealth += diff;
            }

            if (Statistics.MaximumMagic != (TotalWisdom + Level - 1) * 5)
            {
                int diff = ((TotalWisdom + Level - 1) * 5) - Statistics.MaximumMagic;

                Statistics.CurrentMagic += diff;
                Statistics.MaximumMagic += diff;
            }
        }

        #region Experience Manipulation

        /// <summary>Gains experience for Hero.</summary>
        /// <param name="exp">Experience</param>
        /// <returns>Returns text about the Hero gaining experience</returns>
        internal string GainExperience(int exp)
        {
            Experience += exp;
            return $"You gained {exp} experience!{CheckLevelUp()}";
        }

        /// <summary>Checks where a Hero has leveled up.</summary>
        /// <returns>Returns null if Hero doesn't level up</returns>
        private string CheckLevelUp() => Experience >= Level * 100 ? LevelUp() : null;

        /// <summary>Levels up a Hero.</summary>
        /// <returns>Returns text about the Hero leveling up</returns>
        private string LevelUp()
        {
            Experience -= Level * 100;
            Level++;
            SkillPoints += 5;
            Statistics.CurrentHealth += 5;
            Statistics.MaximumHealth += 5;
            Statistics.CurrentMagic += 5;
            Statistics.MaximumMagic += 5;
            Bank.LoanAvailable += 250;
            return "\n\nYou gained a level! You also gained 5 health, 5 magic, and 5 skill points!";
        }

        #endregion Experience Manipulation

        #region Health Manipulation

        /// <summary>The Hero takes damage.</summary>
        /// <param name="damage">Damage amount</param>
        /// <returns>Returns text about the Hero leveling up.</returns>
        internal string TakeDamage(int damage)
        {
            Statistics.CurrentHealth -= damage;
            return Statistics.CurrentHealth <= 0
            ? $"You have taken {damage} damage and have been slain."
            : $"You have taken {damage} damage.";
        }

        /// <summary>Heals the Hero for a specified amount.</summary>
        /// <param name="healAmount">Amount to be healed</param>
        /// <returns>Returns text saying the Hero was healed</returns>
        internal string Heal(int healAmount)
        {
            Statistics.CurrentHealth += healAmount;
            if (Statistics.CurrentHealth > Statistics.MaximumHealth)
            {
                Statistics.CurrentHealth = Statistics.MaximumHealth;
                return "You heal to your maximum health.";
            }
            return $"You heal for {healAmount:N0} health.";
        }

        #endregion Health Manipulation

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

        /// <summary>Equips an Item into a Hero's Equipment.</summary>
        /// <param name="item">Item to be equipped</param>
        /// <param name="side">If Item is a Ring, which side is it?</param>
        internal void Equip(Item item, RingHand side = RingHand.Left)
        {
            switch (item.Type)
            {
                case ItemType.MeleeWeapon | ItemType.RangedWeapon:
                    if (Equipment.Weapon != GameState.DefaultWeapon)
                        AddItem(Equipment.Weapon);
                    Equipment.Weapon = new Item(item);
                    break;

                case ItemType.HeadArmor:
                    if (Equipment.Head != GameState.DefaultHead)
                        AddItem(Equipment.Head);
                    Equipment.Head = new Item(item);
                    break;

                case ItemType.BodyArmor:
                    if (Equipment.Body != GameState.DefaultBody)
                        AddItem(Equipment.Body);
                    Equipment.Body = new Item(item);
                    break;

                case ItemType.HandArmor:
                    if (Equipment.Hands != GameState.DefaultHands)
                        AddItem(Equipment.Hands);
                    Equipment.Hands = new Item(item);
                    break;

                case ItemType.LegArmor:
                    if (Equipment.Legs != GameState.DefaultLegs)
                        AddItem(Equipment.Legs);
                    Equipment.Legs = new Item(item);
                    break;

                case ItemType.FeetArmor:
                    if (Equipment.Feet != GameState.DefaultFeet)
                        AddItem(Equipment.Feet);
                    Equipment.Feet = new Item(item);
                    break;

                case ItemType.Ring:
                    switch (side)
                    {
                        case RingHand.Left:
                            if (Equipment.LeftRing != new Item())
                                AddItem(Equipment.LeftRing);
                            Equipment.LeftRing = new Item(item);
                            break;

                        case RingHand.Right:
                            if (Equipment.RightRing != new Item())
                                AddItem(Equipment.RightRing);
                            Equipment.RightRing = new Item(item);
                            break;
                    }
                    break;

                default:
                    //GameState.DisplayNotification("You have attempted to equip an Item which doesn't fit a current type of item to be equipped.", "Sulimn");
                    break;
            }

            RemoveItem(item);
        }

        /// <summary>Unequips an Item from a Hero's Equipment.</summary>
        /// <param name="item">Item to be unequipped</param>
        /// <param name="side">If Item is a Ring, which side is it?</param>
        internal void Unequip(Item item, RingHand side = RingHand.Left)
        {
            switch (item.Type)
            {
                case ItemType.MeleeWeapon | ItemType.RangedWeapon:
                    if (item != GameState.DefaultWeapon)
                        AddItem(item);
                    Equipment.Weapon = new Item(GameState.DefaultWeapon);
                    break;

                case ItemType.HeadArmor:
                    if (item != GameState.DefaultHead)
                        AddItem(item);
                    Equipment.Head = new Item(GameState.DefaultHead);
                    break;

                case ItemType.BodyArmor:
                    if (item != GameState.DefaultBody)
                        AddItem(item);
                    Equipment.Body = new Item(GameState.DefaultBody);
                    break;

                case ItemType.HandArmor:
                    if (item != GameState.DefaultHands)
                        AddItem(item);
                    Equipment.Hands = new Item(GameState.DefaultHands);
                    break;

                case ItemType.LegArmor:
                    if (item != GameState.DefaultLegs)
                        AddItem(item);
                    Equipment.Legs = new Item(GameState.DefaultLegs);
                    break;

                case ItemType.FeetArmor:
                    if (item != GameState.DefaultFeet)
                        AddItem(item);
                    Equipment.Feet = new Item(GameState.DefaultFeet);
                    break;

                case ItemType.Ring:
                    if (item != new Item())
                        AddItem(item);
                    switch (side)
                    {
                        case RingHand.Left:
                            Equipment.LeftRing = new Item();
                            break;

                        case RingHand.Right:
                            Equipment.RightRing = new Item();
                            break;
                    }
                    break;
            }
        }

        /// <summary>Gets all Items of specified Type.</summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Items of specified Type</returns>
        internal List<T> GetItemsOfType<T>() => Inventory.OfType<T>().ToList();

        #endregion Inventory Management

        #region Override Operators

        public static bool Equals(Hero left, Hero right)
        {
            if (left is null && right is null) return true;
            if (left is null ^ right is null) return false;
            return string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase)
                && left.Level == right.Level
                && left.Experience == right.Experience
                && left.SkillPoints == right.SkillPoints
                && left.Hardcore == right.Hardcore
                && left.Spellbook == right.Spellbook
                && left.Class == right.Class
                && left.Attributes == right.Attributes
                && left.Equipment == right.Equipment
                && left.Gold == right.Gold
                && left.Statistics == right.Statistics
                && left.Progression == right.Progression
                && !left.Inventory.Except(right.Inventory).Any();
        }

        public sealed override bool Equals(object obj) => Equals(this, obj as Hero);

        public bool Equals(Hero otherHero) => Equals(this, otherHero);

        public static bool operator ==(Hero left, Hero right) => Equals(left, right);

        public static bool operator !=(Hero left, Hero right) => !Equals(left, right);

        public sealed override int GetHashCode() => base.GetHashCode() ^ 17;

        public sealed override string ToString() => Name;

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a default instance of Hero.</summary>
        internal Hero()
        {
        }

        /// <summary>Initializes an instance of Hero by assigning Properties.</summary>
        /// <param name="name">Name of Hero</param>
        /// <param name="password">Password of Hero</param>
        /// <param name="heroClass">Class of Hero</param>
        /// <param name="level">Level of Hero</param>
        /// <param name="experience">Experience of Hero</param>
        /// <param name="skillPoints">Skill Points of Hero</param>
        /// <param name="gold">Gold of Hero</param>
        /// <param name="attributes">Attributes of Hero</param>
        /// <param name="statistics">Statistics of Hero</param>
        /// <param name="equipment">Equipment of Hero</param>
        /// <param name="spellbook">Spellbook of Hero</param>
        /// <param name="inventory">Inventory of Hero</param>
        /// <param name="bank">Bank of the Hero</param>
        /// <param name="progression">The progress the Hero has made</param>
        /// <param name="hardcore">Will the character be deleted on death?</param>
        internal Hero(string name, string password, HeroClass heroClass, int level, int experience, int skillPoints, int gold,
        Attributes attributes, Statistics statistics, Equipment equipment, Spellbook spellbook, List<Item> inventory, Bank bank, Progression progression, bool hardcore)
        {
            Name = name;
            Password = password;
            Class = heroClass;
            Level = level;
            Experience = experience;
            Gold = gold;
            SkillPoints = skillPoints;
            Attributes = attributes;
            Statistics = statistics;
            Equipment = equipment;
            Spellbook = spellbook;
            Inventory = inventory;
            Bank = bank;
            Progression = progression;
            Hardcore = hardcore;
        }

        /// <summary>Replaces this instance of Hero with another instance.</summary>
        /// <param name="other">Instance of Hero to replace this one</param>
        internal Hero(Hero other) : this(other.Name, other.Password, other.Class, other.Level, other.Experience, other.SkillPoints, other.Gold, new Attributes(other.Attributes), new Statistics(other.Statistics), new Equipment(other.Equipment), new Spellbook(other.Spellbook), new List<Item>(other.Inventory), other.Bank, other.Progression, other.Hardcore)
        {
        }

        #endregion Constructors
    }
}