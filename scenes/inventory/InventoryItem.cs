using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sulimn.Scenes.Inventory
{
    public class InventoryItem : Control
    {
        public bool Drag;
        private Orphanage orphanage;
        private Item _item = new Item();

        public Item Item
        {
            get => _item;
            set => SetItem(value);
        }

        public override void _Ready()
        {
            orphanage = (Orphanage)GetTree().CurrentScene.FindNode("Orphanage");
            MouseDefaultCursorShape = CursorShape.PointingHand;
        }

        private void _on_TextureRect_gui_input(InputEvent @event)
        {
            if (@event is InputEventMouseButton button && button.Pressed && button.ButtonIndex == 1)
            {
                if (!Drag)
                {
                    ItemSlot slot = (ItemSlot)GetParent();
                    if (orphanage.GetChildCount() > 0)
                    {
                        InventoryItem orphanItem = (InventoryItem)orphanage.GetChild(0);
                        if (!orphanage.PreviousSlot.Merchant && !slot.Merchant && slot.ItemTypes.Contains(orphanItem.Item.Type)) // if not buying nor selling, and is a valid slot
                            SwapItems(slot, orphanItem);
                        else if (orphanage.PreviousSlot.Merchant && !slot.Merchant && GameState.CurrentHero.PurchaseItem(orphanItem.Item)) // if purchasing and can afford it
                            SwapItems(slot, orphanItem);
                        else if (!orphanage.PreviousSlot.Merchant && slot.Merchant) // if selling
                        {
                            GameState.CurrentHero.SellItem(orphanItem.Item);
                            SwapItems(slot, orphanItem);
                        }
                        else if (orphanage.PreviousSlot.Merchant && slot.Merchant) // if moving Merchant item to different Merchant slot
                            SwapItems(slot, orphanItem);
                    }
                    else if (orphanage.GetChildCount() == 0)
                    {
                        Drag = true;
                        TextureRect rect = (TextureRect)GetChild(0);
                        rect.MouseFilter = MouseFilterEnum.Ignore;
                        GetParent().RemoveChild(this);
                        orphanage.AddChild(this);
                        orphanage.PreviousSlot = slot;
                    }
                }
            }
        }

        private void SwapItems(ItemSlot slot, InventoryItem orphanItem)
        {
            TextureRect rect = (TextureRect)orphanItem.GetChild(0);
            rect.MouseFilter = MouseFilterEnum.Pass;
            orphanItem.Drag = false;
            orphanItem.RectGlobalPosition = Vector2.Zero;
            orphanage.RemoveChild(orphanItem);
            slot.AddChild(orphanItem);
            slot.Item = orphanItem;
            slot.RemoveChild(this);
            orphanage.AddChild(this);
            orphanage.PreviousSlot = slot;
            TextureRect rect2 = (TextureRect)GetChild(0);
            rect2.MouseFilter = MouseFilterEnum.Ignore;

            GameState.UpdateDisplay = true;
            Drag = true;
        }

        public override void _Process(float delta)
        {
            if (Drag)
                RectGlobalPosition = GetViewport().GetMousePosition() + new Vector2(1, 1);
        }

        public void SetItem(Item item)
        {
            if (item != null && item != new Item())
            {
                _item = item;
                TextureRect rect = (TextureRect)GetNode("TextureRect");
                SetTooltip(item.TooltipText);
                rect.Texture = (Texture)ResourceLoader.Load(item.Texture);
            }
        }
    }
}