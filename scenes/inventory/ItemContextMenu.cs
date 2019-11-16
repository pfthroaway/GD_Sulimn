using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Items;

namespace Sulimn.Scenes.Inventory
{
    public class ItemContextMenu : PopupMenu
    {
        public ItemSlot CurrentSlot { get; set; }
        private Button BtnConsume, BtnDrop;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            BtnConsume = (Button)GetNode("BtnConsume");
            BtnDrop = (Button)GetNode("BtnDrop");
        }

        public void LoadSlot(ItemSlot slot)
        {
            CurrentSlot = slot;
            if (!slot.Merchant && !slot.Enemy)
            {
                switch (slot.Item.Item.Type)
                {
                    case ItemType.Food:
                    case ItemType.Drink:
                    case ItemType.Potion:
                        BtnConsume.Disabled = false;
                        break;
                }
            }
            else
            {
                BtnConsume.Disabled = true;
                BtnDrop.Disabled = true;
            }
        }

        private void Drop()
        {
            CurrentSlot.RemoveChild(CurrentSlot.Item);
            CurrentSlot.Item = new InventoryItem();
            GameState.UpdateDisplay = true;
        }

        private void _on_BtnConsume_pressed()
        {
            GameState.CurrentHero.ConsumeItem(CurrentSlot.Item.Item);
            Drop();
        }

        private void _on_BtnDrop_pressed() => Drop();
    }
}