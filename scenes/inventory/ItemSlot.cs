using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sulimn.Scenes.Inventory
{
    public class ItemSlot : Control
    {
        private Orphanage orphanage;

        public List<ItemType> ItemTypes { get; set; } = new List<ItemType>(Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToList());

        /// <summary><see cref="InventoryItem"/> currently occupying the <see cref="ItemSlot"/>.</summary>
        public InventoryItem Item { get; set; } = new InventoryItem();

        /// <summary>Is this <see cref="ItemSlot"/> an equipment slot?</summary>
        public bool Equipment { get; set; }

        /// <summary>Is this <see cref="ItemSlot"/> part of an enemy's inventory?</summary>
        public bool Enemy { get; set; }

        /// <summary>Is this <see cref="ItemSlot"/> part of a merchant's inventory?</summary>
        public bool Merchant { get; set; }

        public override void _Ready() => orphanage = (Orphanage)GetTree().CurrentScene.FindNode("Orphanage");

        private void _on_TextureRect_gui_input(InputEvent @event)
        {
            if (@event is InputEventMouseButton button && button.Pressed && button.ButtonIndex == 1)
            {
                if (orphanage.GetChildCount() > 0)
                {
                    InventoryItem item = (InventoryItem)orphanage.GetChild(0);
                    if (!orphanage.PreviousSlot.Merchant && !Merchant && ItemTypes.Contains(item.Item.Type))
                        PutItemInSlot(item);// if not buying nor selling, and is a valid slot
                    else if (orphanage.PreviousSlot.Merchant && !Merchant && GameState.CurrentHero.PurchaseItem(item.Item)) // if purchasing and can afford it
                        PutItemInSlot(item);
                    else if (!orphanage.PreviousSlot.Merchant && Merchant && item.Item.CanSell) // if selling
                    {
                        GameState.CurrentHero.SellItem(item.Item);
                        PutItemInSlot(item);
                    }
                    else if (orphanage.PreviousSlot.Merchant && Merchant) // if moving Merchant item to different Merchant slot
                        PutItemInSlot(item);
                }
            }
        }

        /// <summary>Puts the currently held <see cref="Inventory"/> into the <see cref="ItemSlot"/>.</summary>
        /// <param name="item"><see cref="InventoryItem"/> to be put into the <see cref="ItemSlot"/></param>
        private void PutItemInSlot(InventoryItem item)
        {
            item.Drag = false;
            item.RectGlobalPosition = Vector2.Zero;
            orphanage.RemoveChild(item);
            GameState.UpdateDisplay = true;
            orphanage.PreviousSlot = null;
            AddChild(item);
            Item = item;
            TextureRect rect = (TextureRect)GetChild(1).GetChild(0);
            rect.MouseFilter = MouseFilterEnum.Pass;
        }
    }
}