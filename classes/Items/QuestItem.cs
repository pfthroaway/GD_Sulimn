namespace Sulimn.Classes.Items
{
    /// <summary>Represents a collectible item used for <see cref="Quest"/>s</summary>
    public class QuestItem
    {
        /// <summary>The name of the <see cref="QuestItem"/> being collected.</summary>
        public string Name { get; set; }

        /// <summary>The count of the <see cref="QuestItem"/> being collected.</summary>
        public int Count { get; set; }

        #region Constructors

        /// <summary>Initializes a default instance of <see cref="QuestItem"/>.</summary>
        public QuestItem()
        {
        }

        /// <summary>Initializes an instance of <see cref="QuestItem"/> by assigning Property values.</summary>
        /// <param name="name">The name of the <see cref="QuestItem"/> being collected</param>
        /// <param name="count">The count of the <see cref="QuestItem"/> being collected</param>
        public QuestItem(string name, int count = 0)
        {
            Name = name;
            Count = count;
        }

        /// <summary>Replaces an instance of <see cref="QuestItem"/> with another instance.</summary>
        /// <param name="other">Instance of <see cref="QuestItem"/> to replace this instance</param>
        public QuestItem(QuestItem other) : this(other.Name, other.Count) { }

        #endregion Constructors
    }
}