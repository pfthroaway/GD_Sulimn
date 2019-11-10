using Newtonsoft.Json;
using Sulimn.Classes.Items;
using System.Collections.Generic;

namespace Sulimn.Classes.HeroParts
{
    /// <summary>Represents pieces of equipment an entity is using.</summary>
    public class Equipment
    {
        #region Modifying Properties

        /// <summary>The Weapon an entity is using.</summary>
        [JsonProperty(Order = 1)]
        public Item Weapon { get; set; } = new Item();

        /// <summary>The Head Armor an entity is wearing.</summary>
        [JsonProperty(Order = 2)]
        public Item Head { get; set; } = new Item();

        /// <summary>The Body Armor an entity is wearing.</summary>
        [JsonProperty(Order = 3)]
        public Item Body { get; set; } = new Item();

        /// <summary>The Hand Armor an entity is wearing.</summary>
        [JsonProperty(Order = 4)]
        public Item Hands { get; set; } = new Item();

        /// <summary>The Leg Armor an entity is wearing.</summary>
        [JsonProperty(Order = 5)]
        public Item Legs { get; set; } = new Item();

        /// <summary>The Feet Armor an entity is wearing.</summary>
        [JsonProperty(Order = 6)]
        public Item Feet { get; set; } = new Item();

        /// <summary>The Ring an entity is wearing on its left hand.</summary>
        [JsonProperty(Order = 7)]
        public Item LeftRing { get; set; } = new Item();

        /// <summary>The Ring an entity is wearing on its right hand.</summary>
        [JsonProperty(Order = 8)]
        public Item RightRing { get; set; } = new Item();

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>List of all the <see cref="Item"/>s equipped.</summary>
        [JsonIgnore]
        public List<Item> AllEquipment => new List<Item> { Weapon, Head, Body, Hands, Legs, Feet, LeftRing, RightRing };

        /// <summary>Weight of all the Equipment currently equipped.</summary>
        [JsonIgnore]
        public int TotalWeight => Weapon.Weight + Head.Weight + Body.Weight + Hands.Weight + Legs.Weight
                                  + Feet.Weight + LeftRing.Weight + RightRing.Weight;

        /// <summary>Returns the total damage produced by the current set of equipment.</summary>
        [JsonIgnore]
        public int TotalDamage => Weapon.Damage + Head.Damage + Body.Damage + Hands.Damage + Legs.Damage
                                  + Feet.Damage + LeftRing.Damage + RightRing.Damage;

        /// <summary>Returns the total defense produced by the current set of equipment.</summary>
        [JsonIgnore]
        public int TotalDefense => Weapon.Defense + Head.Defense + Body.Defense + Hands.Defense + Legs.Defense + Feet.Defense + LeftRing.Defense + RightRing.Defense;

        /// <summary>Returns the total damage produced by the current set of equipment with thousand separators.</summary>
        [JsonIgnore]
        public string TotalDefenseToString => TotalDefense.ToString("N0");

        /// <summary>Returns the total damage produced by the current set of equipment with thousand separators and preceding text.</summary>
        [JsonIgnore]
        public string TotalDefenseToStringWithText => $"Defense: {TotalDefense:N0}";

        /// <summary>Returns the total Strength bonus produced by the current set of equipment.</summary>
        [JsonIgnore]
        public int BonusStrength => Weapon.Strength + Head.Strength + Body.Strength + Hands.Strength + Legs.Strength
                                  + Feet.Strength + LeftRing.Strength + RightRing.Strength;

        /// <summary>Returns the total Vitality bonus produced by the current set of equipment.</summary>
        [JsonIgnore]
        public int BonusVitality => Weapon.Vitality + Head.Vitality + Body.Vitality + Hands.Vitality + Legs.Vitality
                                  + Feet.Vitality + LeftRing.Vitality + RightRing.Vitality;

        /// <summary>Returns the total Dexterity bonus produced by the current set of equipment.</summary>
        [JsonIgnore]
        public int BonusDexterity => Weapon.Dexterity + Head.Dexterity + Body.Dexterity + Hands.Dexterity + Legs.Dexterity
                                  + Feet.Dexterity + LeftRing.Dexterity + RightRing.Dexterity;

        /// <summary>Returns the total Wisdom bonus produced by the current set of equipment.</summary>
        [JsonIgnore]
        public int BonusWisdom => Weapon.Wisdom + Head.Wisdom + Body.Wisdom + Hands.Wisdom + Legs.Wisdom
                                  + Feet.Wisdom + LeftRing.Wisdom + RightRing.Wisdom;

        #endregion Helper Properties

        #region Override Operators

        public static bool Equals(Equipment left, Equipment right)
        {
            if (left is null && right is null) return true;
            if (left is null ^ right is null) return false;
            return left.Head == right.Head
                && left.Body == right.Body
                && left.Hands == right.Hands
                && left.Legs == right.Legs
                && left.Feet == right.Feet
                && left.LeftRing == right.LeftRing
                && left.RightRing == right.RightRing
                && left.Weapon == right.Weapon;
        }

        public sealed override bool Equals(object obj) => Equals(this, obj as Equipment);

        public bool Equals(Equipment otherEquipment) => Equals(this, otherEquipment);

        public static bool operator ==(Equipment left, Equipment right) => Equals(left, right);

        public static bool operator !=(Equipment left, Equipment right) => !Equals(left, right);

        public sealed override int GetHashCode() => base.GetHashCode() ^ 17;

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a default instance of Equipment.</summary>
        public Equipment()
        {
        }

        /// <summary>Initializes an instance of Equipment by assigning Properties.</summary>
        /// <param name="weapon">Weapon</param>
        /// <param name="head">Head Armor</param>
        /// <param name="body">Body Armor</param>
        /// <param name="hands">Hand Armor</param>
        /// <param name="legs">Leg Armor</param>
        /// <param name="feet">Feet Armor</param>
        /// <param name="leftRing">Left Ring</param>
        /// <param name="rightRing">Right Ring</param>
        public Equipment(Item weapon, Item head, Item body, Item hands, Item legs, Item feet,
        Item leftRing, Item rightRing)
        {
            Weapon = new Item(weapon);
            Head = new Item(head);
            Body = new Item(body);
            Hands = new Item(hands);
            Legs = new Item(legs);
            Feet = new Item(feet);
            LeftRing = new Item(leftRing);
            RightRing = new Item(rightRing);
        }

        /// <summary>Replaces this instance of Equipment with another instance.</summary>
        /// <param name="other">Instance of Equipment to replace this instance</param>
        public Equipment(Equipment other) : this(new Item(other.Weapon), new Item(other.Head),
            new Item(other.Body), new Item(other.Hands), new Item(other.Legs), new Item(other.Feet),
            new Item(other.LeftRing), new Item(other.RightRing))
        {
        }

        #endregion Constructors
    }
}