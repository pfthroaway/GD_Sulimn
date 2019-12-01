using Sulimn.Classes.Enums;
using Sulimn.Classes.Items;

namespace Sulimn.Classes.HeroParts
{
    /// <summary>Represents a quest that is </summary>
    public class Quest
    {
        #region Properties

        /// <summary>Name of the <see cref="Quest"/>.</summary>
        public string Name { get; set; }

        /// <summary>Description of the <see cref="Quest"/>.</summary>
        public string Description { get; set; }

        /// <summary><see cref="QuestType"/> of the <see cref="Quest"/>.</summary>
        public QuestType QuestType { get; set; }

        /// <summary><see cref="QuestItem"/> required by the <see cref="Quest"/>.</summary>
        public QuestItem QuestItem { get; set; } = new QuestItem();

        #endregion Properties

        #region Constructors

        /// <summary>Initializes a default instance of <see cref="Quest"/>.</summary>
        public Quest()
        {
        }

        /// <summary>Initializes an instance of <see cref="Quest"/> by assigning Property values.</summary>
        /// <param name="name">Name of the <see cref="Quest"/></param>
        /// <param name="description">Description of the <see cref="Quest"/></param>
        /// <param name="type"><see cref="QuestType"/> of the <see cref="Quest"/></param>
        /// <param name="item"><see cref="QuestItem"/> required by the <see cref="Quest"/></param>
        public Quest(string name, string description, QuestType type, QuestItem item)
        {
            Name = name;
            Description = description;
            QuestType = type;
            QuestItem = item;
        }

        /// <summary>Replaces an instance of <see cref="Quest"/> with another instance.</summary>
        /// <param name="other">Instance of <see cref="Quest"/> to replace this instance</param>
        public Quest(Quest other) : this(other.Name, other.Description, other.QuestType, other.QuestItem)
        {
        }

        #endregion Constructors
    }
}