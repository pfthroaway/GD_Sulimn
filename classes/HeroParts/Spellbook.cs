using Sulimn.Classes.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sulimn.Classes.HeroParts
{
    /// <summary>Represents a collection of <see cref="Spell"/>s a <see cref="Hero"/> can cast.</summary>
    internal class Spellbook
    {
        private List<Spell> _spells = new List<Spell>();

        /// <summary>List of known <see cref="Spell"/>s.</summary>
        internal ReadOnlyCollection<Spell> Spells => new ReadOnlyCollection<Spell>(_spells);

        /// <summary>List of known <see cref="Spell"/>s, set up to import from JSON.</summary>
        public string SpellsString
        {
            get => string.Join(",", Spells);
            set
            {
                _spells = new List<Spell>();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    foreach (string spell in value.Split(','))
                        LearnSpell(GameState.AllSpells.Find(spl => spl.Name == spell.Trim()));
                }
            }
        }

        /// <summary>Teaches a <see cref="Hero"/> a <see cref="Spell"/>.</summary>
        /// <param name="newSpell"><see cref="Spell"/> to be learned</param>
        /// <returns>String saying <see cref="Hero"/> learned the <see cref="Spell"/></returns>
        internal string LearnSpell(Spell newSpell)
        {
            _spells.Add(newSpell);
            return $"You learn {newSpell.Name}.";
        }

        #region Override Operators

        public static bool Equals(Spellbook left, Spellbook right)
        {
            if (left is null && right is null) return true;
            if (left is null ^ right is null) return false;
            return !left.Spells.Except(right.Spells).Any()
                && !right.Spells.Except(left.Spells).Any();
        }

        public sealed override bool Equals(object obj) => Equals(this, obj as Spellbook);

        public bool Equals(Spellbook otherSpellbook) => Equals(this, otherSpellbook);

        public static bool operator ==(Spellbook left, Spellbook right) => Equals(left, right);

        public static bool operator !=(Spellbook left, Spellbook right) => !Equals(left, right);

        public sealed override int GetHashCode() => base.GetHashCode() ^ 17;

        public sealed override string ToString() => string.Join(",", Spells);

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a default instance of <see cref="Spellbook"/>.</summary>
        public Spellbook()
        {
        }

        /// <summary>Initializes a new instance of <see cref="Spellbook"/> by assigning known <see cref="Spell"/>s.</summary>
        /// <param name="spellList">List of known <see cref="Spell"/>s</param>
        public Spellbook(IEnumerable<Spell> spellList)
        {
            List<Spell> newSpells = new List<Spell>();
            newSpells.AddRange(spellList);
            _spells = newSpells;
        }

        /// <summary>Replaces this instance of <see cref="Spellbook"/> with another instance.</summary>
        /// <param name="other">Instance of <see cref="Spellbook"/> to replace this instance</param>
        public Spellbook(Spellbook other) : this(new List<Spell>(other.Spells))
        {
        }

        #endregion Constructors
    }
}