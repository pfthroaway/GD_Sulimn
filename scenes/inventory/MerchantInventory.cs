using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Items;
using System.Collections.Generic;

namespace Sulimn.Scenes.Inventory
{
    public class MerchantInventory : Panel
    {
        /// <summary>The count of all the <see cref="Item"/>s in the inventory.</summary>
        public int ItemCount
        {
            get
            {
                int items = 0;
                GridContainer container = (GridContainer)GetNode("GridInventory");
                foreach (ItemSlot slot in container.GetChildren())
                {
                    if (slot.GetChildren().Count == 1)
                        items++;
                }
                return items;
            }
        }

        #region Inventory Manipulation

        /// <summary>Finds the first empty slot in the inventory.</summary>
        /// <returns>The first empty slot in the inventory</returns>
        public ItemSlot FindFirstEmptySlot()
        {
            GridContainer container = (GridContainer)GetNode("GridInventory");
            foreach (ItemSlot slot in container.GetChildren())
            {
                if (slot.GetChildren().Count == 1)
                    return slot;
            }
            return new ItemSlot();
        }

        /// <summary>Sets up an inventory.</summary>
        /// <param name="inventory">Inventory to be set up</param>
        public void SetUpInventory(List<Item> inventory)
        {
            for (int i = 0; i < 80; i++)
            {
                ItemSlot slot = (ItemSlot)FindNode($"ItemSlot{i + 1}");
                slot.Merchant = true;
                if (i < inventory.Count)
                    GameState.AddItemInstanceToSlot(slot, inventory[i]);
            }
        }

        #endregion Inventory Manipulation

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }
    }
}