using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Items;
using System.Collections.Generic;

namespace Sulimn.Scenes.Inventory
{
    public class GridInventory : Panel
    {
        /// <summary>Sets up an inventory.</summary>
        /// <param name="inventory"></param>
        public void SetUpInventory(List<Item> inventory)
        {
            for (int i = 0; i < 40; i++)
            {
                ItemSlot slot = (ItemSlot)FindNode($"ItemSlot{i + 1}");
                if (i < inventory.Count)
                    GameState.AddItemInstanceToSlot(slot, inventory[i]);
            }
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }
    }
}