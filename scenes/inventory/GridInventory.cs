using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Items;
using System.Collections.Generic;

namespace Sulimn.Scenes.Inventory
{
    public class GridInventory : Panel
    {
        /// <summary>Sets up an inventory.</summary>
        /// <param name="inventory">Inventory to be set up</param>
        /// <param name="enemy">Is this an enemy's inventory?</param>
        public void SetUpInventory(List<Item> inventory, bool enemy = false)
        {
            for (int i = 0; i < 40; i++)
            {
                ItemSlot slot = (ItemSlot)FindNode($"ItemSlot{i + 1}");
                slot.Enemy = enemy;
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

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }
    }
}