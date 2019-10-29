using Newtonsoft.Json;
using Sulimn.Classes.Items;

namespace Sulimn.Classes.HeroParts
{
    /// <summary>Represents pieces of equipment an entity is using.</summary>
    internal class Equipment
    {
        #region Modifying Properties

        [JsonProperty(Order = 1)]
        /// <summary>The Weapon an entity is using.</summary>
        public Item Weapon { get; set; } = new Item();

        [JsonProperty(Order = 2)]
        /// <summary>The Head Armor an entity is wearing.</summary>
        public Item Head { get; set; } = new Item();

        [JsonProperty(Order = 3)]
        /// <summary>The Body Armor an entity is wearing.</summary>
        public Item Body { get; set; } = new Item();

        [JsonProperty(Order = 4)]
        /// <summary>The Hand Armor an entity is wearing.</summary>
        public Item Hands { get; set; } = new Item();

        [JsonProperty(Order = 5)]
        /// <summary>The Leg Armor an entity is wearing.</summary>
        public Item Legs { get; set; } = new Item();

        [JsonProperty(Order = 6)]
        /// <summary>The Feet Armor an entity is wearing.</summary>
        public Item Feet { get; set; } = new Item();

        [JsonProperty(Order = 7)]
        /// <summary>The Ring an entity is wearing on its left hand.</summary>
        public Item LeftRing { get; set; } = new Item();

        [JsonProperty(Order = 8)]
        /// <summary>The Ring an entity is wearing on its right hand.</summary>
        public Item RightRing { get; set; } = new Item();

        #endregion Modifying Properties

        #region Helper Properties

        [JsonIgnore]
        /// <summary>Weight of all the Equipment currently equipped.</summary>
        public int TotalWeight => Weapon.Weight + Body.Weight + Head.Weight + Body.Weight + Hands.Weight + Legs.Weight
                                  + Feet.Weight;

        [JsonIgnore]
        /// <summary>Returns the total damage produced by the current set of equipment.</summary>
        public int TotalDamage => Weapon.Damage + LeftRing.Damage + RightRing.Damage;

        [JsonIgnore]
        /// <summary>Returns the total defense produced by the current set of equipment.</summary>
        public int TotalDefense => Head.Defense + Body.Defense + Hands.Defense + Legs.Defense + Feet.Defense + LeftRing.Defense
        + RightRing.Defense;

        [JsonIgnore]
        /// <summary>Returns the total damage produced by the current set of equipment with thousand separators.</summary>
        public string TotalDefenseToString => TotalDefense.ToString("N0");

        [JsonIgnore]
        /// <summary>Returns the total damage produced by the current set of equipment with thousand separators and preceding text.</summary>
        public string TotalDefenseToStringWithText => $"Defense: {TotalDefense:N0}";

        [JsonIgnore]
        /// <summary>Returns the total Strength bonus produced by the current set of equipment.</summary>
        public int BonusStrength => LeftRing != null && RightRing != null ? LeftRing.Strength + RightRing.Strength : 0;

        [JsonIgnore]
        /// <summary>Returns the total Vitality bonus produced by the current set of equipment.</summary>
        public int BonusVitality => LeftRing != null && RightRing != null ? LeftRing.Vitality + RightRing.Vitality : 0;

        [JsonIgnore]
        /// <summary>Returns the total Dexterity bonus produced by the current set of equipment.</summary>
        public int BonusDexterity => LeftRing != null && RightRing != null ? LeftRing.Dexterity + RightRing.Dexterity : 0;

        [JsonIgnore]
        /// <summary>Returns the total Wisdom bonus produced by the current set of equipment.</summary>
        public int BonusWisdom => LeftRing != null && RightRing != null ? LeftRing.Wisdom + RightRing.Wisdom : 0;

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