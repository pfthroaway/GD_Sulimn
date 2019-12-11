using Sulimn.Classes.HeroParts;
using Sulimn.Classes.Items;
using System.Collections.Generic;

namespace Sulimn.Classes.Entities
{
    internal interface ICharacter
    {
        #region Modifying Properties

        /// <summary>Name of the <see cref="Character"/>.</summary>
        string Name { get; set; }

        /// <summary>Level of the <see cref="Character"/>.</summary>
        int Level { get; set; }

        /// <summary>Name of the <see cref="Character"/>.</summary>
        int Experience { get; set; }

        /// <summary>Amount of Gold in the <see cref="Character"/>'s inventory.</summary>
        int Gold { get; set; }

        /// <summary>Attributes of the <see cref="Character"/>.</summary>
        Attributes Attributes { get; set; }

        /// <summary>Statistics of the <see cref="Character"/>.</summary>
        Statistics Statistics { get; set; }

        /// <summary>Equipment of the <see cref="Character"/>.</summary>
        Equipment Equipment { get; set; }

        /// <summary>The list of <see cref="Spell"/>s the <see cref="Character"/> currently knows.</summary>
        Spellbook Spellbook { get; set; }

        /// <summary>Currently prepared <see cref="Spell"/>.</summary>
        Spell CurrentSpell { get; set; }

        /// <summary>Currently prepared <see cref="Spell"/>, to string.</summary>
        string CurrentSpellString { get; set; }

        /// <summary>List of <see cref="Item"/>s in the <see cref="Character"/>'s inventory.</summary>
        List<Item> Inventory { get; set; }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>Returns the <see cref="Character"/>'s level with preceding text.</summary>
        string LevelToString { get; }

        /// <summary>The experience the <see cref="Character"/> has gained this level alongside how much is needed to level up.</summary>
        string ExperienceToString { get; }

        /// <summary>The experience the <see cref="Character"/> has gained this level alongside how much is needed to level up with preceding text.</summary>
        string ExperienceToStringWithText { get; }

        /// <summary>Amount of Gold in the <see cref="Character"/>'s inventory, with thousands separator.</summary>
        string GoldToString { get; }

        /// <summary>Amount of Gold in the <see cref="Character"/>'s inventory, with thousands separator and preceding text.</summary>
        string GoldToStringWithText { get; }

        /// <summary>List of <see cref="Item"/>s in the <see cref="Character"/>'s inventory, formatted.</summary>
        string InventoryToString { get; }

        /// <summary>Combined weight of all <see cref="Item"/>s in a <see cref="Character"/>'s inventory.</summary>
        int CarryingWeight { get; }

        /// <summary>Combined weight of all <see cref="Item"/>s in a <see cref="Character"/>'s inventory and all the Equipment currently equipped.</summary>
        int TotalWeight { get; }

        /// <summary>Combined weight of all <see cref="Items"/>s in a <see cref="Character"/>'s inventory and all the Equipment currently equipped, formatted.</summary>
        string TotalWeightToString { get; }

        /// <summary>Maximum weight a <see cref="Character"/> can carry.</summary>
        int MaximumWeight { get; }

        /// <summary>Maximum weight a <see cref="Character"/> can carry, formatted.</summary>
        string MaximumWeightToString { get; }

        /// <summary>Displays the <see cref="Character"/>'s currently held weight alongside the maximum weight.</summary>
        string WeightToString { get; }

        /// <summary>Is the <see cref="Character"/> carrying more than they should be able to?</summary>
        bool Overweight { get; }

        /// <summary>Displays text regarding the <see cref="Character"/> carrying more than they should be able to carry.</summary>
        string OverweightToString { get; }

        /// <summary>Ratio of Total Strength to Total Weight.</summary>
        decimal StrengthWeightRatio { get; }

        /// <summary>Total Strength value including from Attributes and Equipment.</summary>
        int TotalStrength { get; }

        /// <summary>Total Vitality value including from Attributes and Equipment.</summary>
        int TotalVitality { get; }

        /// <summary>Total Dexterity value including from Attributes and Equipment.</summary>
        int TotalDexterity { get; }

        /// <summary>Total Wisdom value including from Attributes and Equipment.</summary>
        int TotalWisdom { get; }

        #endregion Helper Properties

        #region Inventory Management

        /// <summary>Adds an <see cref="Item"/> to the inventory.</summary>
        /// <param name="item"><see cref="Item"/> to be removed</param>
        void AddItem(Item item);

        /// <summary>Removes an <see cref="Item"/> from the inventory.</summary>
        /// <param name="item"><see cref="Item"/> to be removed</param>
        void RemoveItem(Item item);

        #endregion Inventory Management
    }
}