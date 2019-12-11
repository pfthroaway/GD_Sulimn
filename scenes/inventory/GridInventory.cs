using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Entities;
using Sulimn.Classes.Items;
using System.Collections.Generic;

namespace Sulimn.Scenes.Inventory
{
    public class GridInventory : Panel
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

        /// <summary>Finds the first empty slot available in the Grid.</summary>
        /// <returns>Returns the first empty slot available in the Grid</returns>
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
        /// <param name="enemy">Is this an <see cref="Enemy"/>'s inventory?</param>
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

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }
    }
}