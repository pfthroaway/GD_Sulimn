using System.Collections.Generic;

namespace Sulimn.Classes.Items
{
    /// <summary>Represents a collectible item used for <see cref="Quest"/>s</summary>
    public class QuestItem
    {
        #region Modifying Properties

        /// <summary>The name of the <see cref="QuestItem"/> being collected.</summary>
        public string Name { get; set; }

        /// <summary>List of available locations where the <see cref="QuestItem"/> can be collected.</summary>
        public List<string> AvailableLocations { get; set; }

        /// <summary>The required count of the <see cref="QuestItem"/> being collected.</summary>
        public int RequiredCount { get; set; }

        /// <summary>The current count of the <see cref="QuestItem"/> being collected.</summary>
        public int CurrentCount { get; set; }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>Is the <see cref="QuestItem"/> complete?</summary>
        public bool IsComplete => CurrentCount >= RequiredCount;

        #endregion Helper Properties

        #region Constructors

        /// <summary>Initializes a default instance of <see cref="QuestItem"/>.</summary>
        public QuestItem()
        {
        }

        /// <summary>Initializes an instance of <see cref="QuestItem"/> by assigning Property values.</summary>
        /// <param name="name">The name of the <see cref="QuestItem"/> being collected</param>
        /// <param name="requiredCount">The required count of the <see cref="QuestItem"/> being collected</param>
        /// <param name="currentCount">The current count of the <see cref="QuestItem"/> being collected</param>
        public QuestItem(string name, List<string> availableLocations, int requiredCount, int currentCount = 0)
        {
            Name = name;
            AvailableLocations = availableLocations;
            RequiredCount = requiredCount;
            CurrentCount = currentCount;
        }

        /// <summary>Replaces an instance of <see cref="QuestItem"/> with another instance.</summary>
        /// <param name="other">Instance of <see cref="QuestItem"/> to replace this instance</param>
        public QuestItem(QuestItem other) : this(other.Name, other.AvailableLocations, other.RequiredCount, other.CurrentCount) { }

        #endregion Constructors
    }
}