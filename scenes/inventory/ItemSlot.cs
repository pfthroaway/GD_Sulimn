using Godot;
using Sulimn.Classes;
using Sulimn.Classes.HeroParts;
using Sulimn.Classes.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sulimn.Scenes.Inventory
{
    /// <summary>Represents a slot to put <see cref="InventoryItem"/>s in.</summary>
    public class ItemSlot : Control
    {
        private Orphanage orphanage;
        public Label LblError;

        public List<ItemType> ItemTypes { get; set; } = new List<ItemType>(Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToList());

        /// <summary>Current <see cref="HeroClass"/> of Items allowed in the <see cref="ItemSlot"/> </summary>
        public HeroClass CurrentClass = new HeroClass();

        /// <summary><see cref="InventoryItem"/> currently occupying the <see cref="ItemSlot"/>.</summary>
        public InventoryItem Item { get; set; } = new InventoryItem();

        /// <summary>Is this <see cref="ItemSlot"/> an equipment slot?</summary>
        public bool Equipment { get; set; }

        /// <summary>Is this <see cref="ItemSlot"/> part of an enemy's inventory?</summary>
        public bool Enemy { get; set; }

        /// <summary>Maximum level of <see cref="Item"/>s allowed in <see cref="ItemSlot"/>.</summary>
        public int MaximumItemLevel { get; set; }

        /// <summary>Is this <see cref="ItemSlot"/> part of a merchant's inventory?</summary>
        public bool Merchant { get; set; }

        public override void _Ready()
        {
            orphanage = (Orphanage)GetTree().CurrentScene.FindNode("Orphanage");
            LblError = (Label)GetTree().CurrentScene.FindNode("LblError");
        }

        private void _on_TextureRect_gui_input(InputEvent @event)
        {
            if (@event is InputEventMouseButton button && button.Pressed && button.ButtonIndex == 1 && orphanage.GetChildCount() > 0)
            {
                LblError.Text = "";
                InventoryItem orphanItem = (InventoryItem)orphanage.GetChild(0);
                if (ItemTypes.Contains(orphanItem.Item.Type))
                {
                    if (!orphanage.PreviousSlot.Merchant && !Merchant && (MaximumItemLevel == 0 || MaximumItemLevel >= orphanItem.Item.MinimumLevel) && (orphanItem.Item.AllowedClasses.Count == 0 || CurrentClass == null || CurrentClass == new HeroClass() || orphanItem.Item.AllowedClasses.Contains(CurrentClass)))
                        PutItemInSlot(orphanItem);// if not buying nor selling, and is a valid slot
                    else if (orphanage.PreviousSlot.Merchant && !Merchant && (MaximumItemLevel == 0 || MaximumItemLevel >= orphanItem.Item.MinimumLevel) && (orphanItem.Item.AllowedClasses.Count == 0 || CurrentClass == null || CurrentClass == new HeroClass() || orphanItem.Item.AllowedClasses.Contains(CurrentClass)) && GameState.CurrentHero.PurchaseItem(orphanItem.Item)) // if purchasing and can afford it
                        PutItemInSlot(orphanItem);
                    else if (!orphanage.PreviousSlot.Merchant && Merchant && orphanItem.Item.CanSell) // if selling
                    {
                        GameState.CurrentHero.SellItem(orphanItem.Item);
                        PutItemInSlot(orphanItem);
                    }
                    else if (orphanage.PreviousSlot.Merchant && Merchant) // if moving Merchant item to different Merchant slot
                        PutItemInSlot(orphanItem);
                    else if (MaximumItemLevel < orphanItem.Item.MinimumLevel)
                        LblError.Text = "You are not high enough level to equip this item.";
                    else if (CurrentClass != new HeroClass() && !orphanItem.Item.AllowedClasses.Contains(CurrentClass))
                        LblError.Text = "You are unable to equip this item because you are not the correct class.";
                    // TODO Put in more errors for why you can't do certain things.
                }
            }
        }

        /// <summary>Puts the currently held <see cref="Inventory"/> into the <see cref="ItemSlot"/>.</summary>
        /// <param name="item"><see cref="InventoryItem"/> to be put into the <see cref="ItemSlot"/></param>
        public void PutItemInSlot(InventoryItem item)
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