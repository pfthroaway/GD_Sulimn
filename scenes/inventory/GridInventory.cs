using Godot;
using Sulimn.Classes.Items;
using System.Collections.Generic;

namespace Sulimn.Scenes.Inventory
{
    public class GridInventory : Panel
    {
        private void AddItemInstanceToSlot(ItemSlot slot, Item item)
        {
            var scene = (PackedScene)ResourceLoader.Load("res://scenes/inventory/InventoryItem.tscn");
            InventoryItem invItem = (InventoryItem)scene.Instance();
            slot.AddChild(invItem);
            slot.Item = invItem;
            slot.Item.SetItem(item);
            invItem.Theme = (Theme)ResourceLoader.Load("res://resources/TextureRect.tres");
        }

        /// <summary>Sets up an inventory.</summary>
        /// <param name="inventory"></param>
        public void SetUpInventory(List<Item> inventory, bool merchant)
        {
            for (int i = 0; i < 40; i++)
            {
                ItemSlot slot = (ItemSlot)FindNode($"ItemSlot{i + 1}");
                slot.Merchant = merchant;
                if (i < inventory.Count)
                    AddItemInstanceToSlot(slot, inventory[i]);
            }
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }
    }
}