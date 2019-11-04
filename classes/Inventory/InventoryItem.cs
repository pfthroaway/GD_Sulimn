using Godot;
using Sulimn.Classes.Items;

namespace Sulimn.Classes.Inventory
{
    /// <summary>Represents an <see cref="Item"/> being shown in an Inventory screen.</summary>
    public class InventoryItem : TextureRect
    {
        #region Properties

        /// <summary><see cref="Item"/> to be the focus of this <see cref="InventoryItem"/>.</summary>
        public Item Item { get; set; } = new Item();

        /// <summary><see cref="ItemSlot"/> of this <see cref="InventoryItem"/>.</summary>
        public ItemSlot Slot { get; set; } = new ItemSlot(0);

        /// <summary>Has this <see cref="InventoryItem"/> been picked up?</summary>
        public bool Picked { get; set; }

        #endregion Properties

        #region Item Manipulation

        /// <summary>Picks up an <see cref="InventoryItem"/>.</summary>
        public void PickItem()
        {
            MouseFilter = MouseFilterEnum.Ignore;
            Picked = true;
        }

        /// <summary>Puts an <see cref="InventoryItem"/> into an <see cref="ItemSlot"/>.</summary>
        public void PutItem()
        {
            RectGlobalPosition = new Vector2(0, 0);
            MouseFilter = MouseFilterEnum.Pass;
            Picked = false;
        }

        #endregion Item Manipulation

        /// <summary>Initializes an instance of <see cref="InventoryItem"/> by assigning Property values.</summary>
        /// <param name="item"><see cref="Item"/> to be the focus of this <see cref="InventoryItem"/></param>
        /// <param name="slot"><see cref="ItemSlot"/> of this <see cref="InventoryItem"/></param>
        public InventoryItem(Item item, ItemSlot slot)
        {
            Item = item;
            Name = item.Name;
            Texture = (Texture)ResourceLoader.Load(item.Texture);
            Slot = slot;
            HintTooltip = item.TooltipText;
            MouseFilter = MouseFilterEnum.Pass;
            MouseDefaultCursorShape = CursorShape.PointingHand;
        }
    }
}