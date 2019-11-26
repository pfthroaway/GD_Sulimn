using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Items;
using System.Collections.Generic;

namespace Sulimn.Scenes.Inventory
{
    public class MerchantInventory : Panel
    {
        /// <summary>Sets up an inventory.</summary>
        /// <param name="inventory"></param>
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

        public int GetItemsInInventory()
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

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }
    }
}