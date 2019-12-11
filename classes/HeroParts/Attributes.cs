using Newtonsoft.Json;

namespace Sulimn.Classes.HeroParts
{
    /// <summary>Represents the <see cref="Attributes"/> an entity has.</summary>
    internal class Attributes
    {
        #region Modifying Properties

        /// <summary>How strong an entity is.</summary>
        [JsonProperty(Order = 1)]
        public int Strength { get; set; }

        /// <summary>How much health an entity can have.</summary>
        [JsonProperty(Order = 2)]
        public int Vitality { get; set; }

        /// <summary>How fast an entity can move.</summary>
        [JsonProperty(Order = 3)]
        public int Dexterity { get; set; }

        /// <summary>How magically-inclined an entity is.</summary>
        [JsonProperty(Order = 4)]
        public int Wisdom { get; set; }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>How strong an entity is, formatted.</summary>
        [JsonIgnore]
        public string StrengthToString => Strength.ToString("N0");

        /// <summary>How strong an entity is, formatted with preceding text.</summary>
        [JsonIgnore]
        public string StrengthToStringWithText => $"Strength: {StrengthToString}";

        /// <summary>How much health an entity can have, formatted.</summary>
        [JsonIgnore]
        public string VitalityToString => Vitality.ToString("N0");

        /// <summary>How much health an entity can have, formatted.</summary>
        [JsonIgnore]
        public string VitalityToStringWithText => $"Vitality: {VitalityToString}";

        /// <summary>How fast an entity can move, formatted.</summary>
        [JsonIgnore]
        public string DexterityToString => Dexterity.ToString("N0");

        /// <summary>How fast an entity can move, formatted.</summary>
        [JsonIgnore]
        public string DexterityToStringWithText => $"Dexterity: {DexterityToString}";

        /// <summary>How magically-inclined an entity is, formatted.</summary>
        [JsonIgnore]
        public string WisdomToString => Wisdom.ToString("N0");

        /// <summary>How magically-inclined an entity is, formatted.</summary>
        [JsonIgnore]
        public string WisdomToStringWithText => $"Wisdom: {WisdomToString}";

        #endregion Helper Properties

        #region Override Operators

        public static bool Equals(Attributes left, Attributes right)
        {
            if (left is null && right is null) return true;
            if (left is null ^ right is null) return false;
            return left.Strength == right.Strength && left.Vitality == right.Vitality && left.Dexterity == right.Dexterity && left.Wisdom == right.Wisdom;
        }

        public sealed override bool Equals(object obj) => Equals(this, obj as Attributes);

        public bool Equals(Attributes otherAttributes) => Equals(this, otherAttributes);

        public static bool operator ==(Attributes left, Attributes right) => Equals(left, right);

        public static bool operator !=(Attributes left, Attributes right) => !Equals(left, right);

        public sealed override int GetHashCode() => base.GetHashCode() ^ 17;

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a default instance of <see cref="Attributes"/>.</summary>
        public Attributes()
        {
        }

        /// <summary>Initializes an instance of <see cref="Attributes"/> by assigning Properties.</summary>
        /// <param name="strength">Strength</param>
        /// <param name="vitality">Vitality</param>
        /// <param name="dexterity">Dexterity</param>
        /// <param name="wisdom">Wisdom</param>
        public Attributes(int strength, int vitality, int dexterity, int wisdom)
        {
            Strength = strength;
            Vitality = vitality;
            Dexterity = dexterity;
            Wisdom = wisdom;
        }

        /// <summary>Replaces this instance of <see cref="Attributes"/> with another instance.</summary>
        /// <param name="other">Instance of <see cref="Attributes"/> to replace this instance</param>
        public Attributes(Attributes other) : this(other.Strength, other.Vitality, other.Dexterity, other.Wisdom)
        {
        }

        #endregion Constructors
    }
}