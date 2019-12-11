using Godot;

namespace Sulimn.Scenes.Inventory
{
    /// <summary>Represents a place where orphan <see cref="InventoryItem"/>s are stored temporarily.</summary>
    public class Orphanage : Control
    {
        /// <summary>The <see cref="ItemSlot"/> in which the <see cref="InventoryItem"/> was previously located.</summary>
        public ItemSlot PreviousSlot { get; set; }

        /// <summary>Gets the <see cref="InventoryItem"/> currently stored in the <see cref="Orphanage"/>, if any.</summary>
        /// <returns>The <see cref="InventoryItem"/> currently stored in the <see cref="Orphanage"/>, if any</returns>
        public InventoryItem GetItem()
        {
            if (GetChildren().Count > 0)
                return (InventoryItem)GetChild(0);
            return new InventoryItem();
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }
    }
}