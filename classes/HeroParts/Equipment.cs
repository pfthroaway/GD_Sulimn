using Newtonsoft.Json;
using Sulimn.Classes.Items;

namespace Sulimn.Classes.HeroParts
{
    /// <summary>Represents pieces of equipment an entity is using.</summary>
    internal class Equipment
    {
        #region Modifying Properties

        [JsonIgnore]
        /// <summary>The <see cref="Weapon"/> an entity is using.</summary>
        public Weapon Weapon { get; set; }

        [JsonProperty(Order = 1)]
        /// <summary>The <see cref="Weapon"/> an entity is using, set up to import from JSON.</summary>
        public string WeaponString
        {
            get => Weapon?.Name;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    Weapon = GameState.AllWeapons.Find(o => o.Name == value);
            }
        }

        [JsonIgnore]
        /// <summary>The Head Armor an entity is wearing.</summary>
        public HeadArmor Head { get; set; }

        [JsonProperty(Order = 2)]
        /// <summary>The <see cref="HeadArmor"/> an entity is using, set up to import from JSON.</summary>
        public string HeadArmorString
        {
            get => Head?.Name;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    Head = GameState.AllHeadArmor.Find(o => o.Name == value);
            }
        }

        [JsonIgnore]
        /// <summary>The Body Armor an entity is wearing.</summary>
        public BodyArmor Body { get; set; }

        [JsonProperty(Order = 3)]
        /// <summary>The <see cref="BodyArmor"/> an entity is using, set up to import from JSON.</summary>
        public string BodyArmorString
        {
            get => Body?.Name;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    Body = GameState.AllBodyArmor.Find(o => o.Name == value);
            }
        }

        [JsonIgnore]
        /// <summary>The Hand Armor an entity is wearing.</summary>
        public HandArmor Hands { get; set; }

        [JsonProperty(Order = 4)]
        /// <summary>The <see cref="HandArmor"/> an entity is using, set up to import from JSON.</summary>
        public string HandArmorString
        {
            get => Hands?.Name;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    Hands = GameState.AllHandArmor.Find(o => o.Name == value);
            }
        }

        [JsonIgnore]
        /// <summary>The Leg Armor an entity is wearing.</summary>
        public LegArmor Legs { get; set; }

        [JsonProperty(Order = 5)]
        /// <summary>The <see cref="LegArmor"/> an entity is using, set up to import from JSON.</summary>
        public string LegsArmorString
        {
            get => Legs?.Name;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    Legs = GameState.AllLegArmor.Find(o => o.Name == value);
            }
        }

        [JsonIgnore]
        /// <summary>The Feet Armor an entity is wearing.</summary>
        public FeetArmor Feet { get; set; }

        [JsonProperty(Order = 6)]
        /// <summary>The <see cref="FeetArmor"/> an entity is using, set up to import from JSON.</summary>
        public string FeetArmorString
        {
            get => Feet?.Name;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    Feet = GameState.AllFeetArmor.Find(o => o.Name == value);
            }
        }

        [JsonIgnore]
        /// <summary>The Ring an entity is wearing on its left hand.</summary>
        public Ring LeftRing { get; set; }

        [JsonProperty(Order = 7)]
        /// <summary>The <see cref="LeftRing"/> an entity is using, set up to import from JSON.</summary>
        public string LeftRingString
        {
            get => LeftRing?.Name;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    LeftRing = GameState.AllRings.Find(o => o.Name == value);
            }
        }

        [JsonIgnore]
        /// <summary>The Ring an entity is wearing on its right hand.</summary>
        public Ring RightRing { get; set; }

        [JsonProperty(Order = 8)]
        /// <summary>The <see cref="RightRing"/> an entity is using, set up to import from JSON.</summary>
        public string RightRingString
        {
            get => RightRing?.Name;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    RightRing = GameState.AllRings.Find(o => o.Name == value);
            }
        }

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
        public Equipment(Weapon weapon, HeadArmor head, BodyArmor body, HandArmor hands, LegArmor legs, FeetArmor feet,
        Ring leftRing, Ring rightRing)
        {
            Weapon = new Weapon(weapon);
            Head = new HeadArmor(head);
            Body = new BodyArmor(body);
            Hands = new HandArmor(hands);
            Legs = new LegArmor(legs);
            Feet = new FeetArmor(feet);
            LeftRing = new Ring(leftRing);
            RightRing = new Ring(rightRing);
        }

        /// <summary>Replaces this instance of Equipment with another instance.</summary>
        /// <param name="other">Instance of Equipment to replace this instance</param>
        public Equipment(Equipment other) : this(new Weapon(other.Weapon), new HeadArmor(other.Head),
            new BodyArmor(other.Body), new HandArmor(other.Hands), new LegArmor(other.Legs), new FeetArmor(other.Feet),
            new Ring(other.LeftRing), new Ring(other.RightRing))
        {
        }

        #endregion Constructors
    }
}