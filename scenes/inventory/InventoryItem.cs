using Godot;
using Sulimn.Classes;
using Sulimn.Classes.HeroParts;
using Sulimn.Classes.Items;

namespace Sulimn.Scenes.Inventory
{
    /// <summary>Represents a displayable Item to be stored and displayed in <see cref="ItemSlot"/>s.</summary>
    public class InventoryItem : Control
    {
        public bool Drag;
        private Orphanage orphanage;
        private Item _item = new Item();
        private ItemContextMenu _contextMenu;

        /// <summary>Item held by the <see cref="InventoryItem"/>.</summary>
        public Item Item
        {
            get => _item;
            set => SetItem(value);
        }

        public override void _Ready()
        {
            orphanage = (Orphanage)GetTree().CurrentScene.FindNode("Orphanage");
            _contextMenu = (ItemContextMenu)GetNode("ItemContextMenu");
            MouseDefaultCursorShape = CursorShape.PointingHand;
        }

        /// <summary>Repairs the Item.</summary>
        public void RepairItem()
        {
            if (Item != new Item())
            {
                Item.CurrentDurability = Item.MaximumDurability;
                GameState.UpdateDisplay = true;
            }
        }

        /// <summary>Checks whether the item held in an <see cref="InventoryItem"/> is an appropriate level to be equipped in a particular <see cref="ItemSlot"/>.</summary>
        /// <param name="slot"><see cref="ItemSlot"/> where the <see cref="InventoryItem"/> is attempting to be equipped</param>
        /// <param name="item"><see cref="InventoryItem"/> attempting to be equipped</param>
        /// <returns>True if it is an appropriate level</returns>
        private bool CheckItemLevel(ItemSlot slot, InventoryItem item) => slot.MaximumItemLevel == 0 || slot.MaximumItemLevel >= item.Item.MinimumLevel;

        /// <summary>Checks whether the item held in an <see cref="InventoryItem"/> is an appropriate <see cref="HeroClass"/> to be equipped in a particular <see cref="ItemSlot"/>.</summary>
        /// <param name="slot"><see cref="ItemSlot"/> where the <see cref="InventoryItem"/> is attempting to be equipped</param>
        /// <param name="item"><see cref="InventoryItem"/> attempting to be equipped</param>
        /// <returns>True if it is an appropriate <see cref="HeroClass"/></returns>
        private bool CheckItemClasses(ItemSlot slot, InventoryItem item) => item.Item.AllowedClasses.Count == 0 || slot.CurrentClass == null || slot.CurrentClass == new HeroClass() || item.Item.AllowedClasses.Contains(slot.CurrentClass);

        private void _on_TextureRect_gui_input(InputEvent @event)
        {
            if (@event is InputEventMouseButton button && button.Pressed)
            {
                ItemSlot slot = (ItemSlot)GetParent();
                if (button.ButtonIndex == 1 && !Input.IsKeyPressed((int)KeyList.Control))
                {
                    if (!Drag)
                    {
                        if (orphanage.GetChildCount() > 0)
                        {
                            InventoryItem orphanItem = (InventoryItem)orphanage.GetChild(0);
                            if (slot.ItemTypes.Contains(orphanItem.Item.Type))
                            {
                                if (!orphanage.PreviousSlot.Merchant && !slot.Merchant && CheckItemLevel(slot, orphanItem) && CheckItemClasses(slot, orphanItem)) // if not buying nor selling, and is a valid slot
                                    SwapItems(slot, orphanItem);
                                else if (orphanage.PreviousSlot.Merchant && !slot.Merchant && CheckItemLevel(slot, orphanItem) && CheckItemClasses(slot, orphanItem))
                                    PurchaseItem(slot, orphanItem, true);
                                else if (!orphanage.PreviousSlot.Merchant && slot.Merchant) // if selling
                                    SellItem(slot, orphanItem, true);
                                else if (orphanage.PreviousSlot.Merchant && slot.Merchant) // if moving Merchant item to different Merchant slot
                                    SwapItems(slot, orphanItem);
                                else if (!CheckItemLevel(slot, orphanItem))
                                    slot.LblError.Text = "You are not high enough level to equip this item.";
                                else if (!CheckItemClasses(slot, orphanItem))
                                    slot.LblError.Text = "You are unable to equip this item because you are not the correct class.";
                            }
                            else
                                slot.LblError.Text = "That is not a valid slot for this item.";
                        }
                        else if (orphanage.GetChildCount() == 0)
                        {
                            Drag = true;
                            TextureRect rect = (TextureRect)GetChild(0);
                            rect.MouseFilter = MouseFilterEnum.Ignore;
                            PutItemInOrphanage(slot);
                        }
                    }
                }
                else if (button.ButtonIndex == 1 && Input.IsKeyPressed((int)KeyList.Control))
                {
                    if (GetTree().CurrentScene.Name == "ItemMerchant")
                    {
                        Node container = GetParent().GetParent().GetParent();
                        if (container.Name == "GridInventory")
                        {
                            GridInventory gridInventory = (GridInventory)container;
                            MerchantInventory merchantInventory = (MerchantInventory)GetTree().CurrentScene.FindNode("MerchantInventory");
                            if (merchantInventory != null)
                            {
                                PutItemInOrphanage(slot);
                                SellItem(merchantInventory.FindFirstEmptySlot(), this, false);
                            }
                        }
                        else if (container.Name == "MerchantInventory")
                        {
                            MerchantInventory merchantInventory = (MerchantInventory)container;
                            GridInventory gridInventory = (GridInventory)GetTree().CurrentScene.FindNode("GridInventory");
                            PutItemInOrphanage(slot);
                            PurchaseItem(gridInventory.FindFirstEmptySlot(), this, false);
                            GD.Print("This is a MerchantInventory!");
                        }
                    }
                }
                else if (button.ButtonIndex == 2)
                {
                    _contextMenu.PopupCentered();
                    _contextMenu.SetGlobalPosition(new Vector2(button.GetGlobalPosition().x + 32, button.GetGlobalPosition().y - 32));
                    _contextMenu.LoadSlot(slot);
                }
            }
        }

        private void PutItemInOrphanage(ItemSlot slot)
        {
            GetParent().RemoveChild(this);
            slot.Item = new InventoryItem();
            orphanage.AddChild(this);
            orphanage.PreviousSlot = slot;
        }

        private void PurchaseItem(ItemSlot slot, InventoryItem purchaseItem, bool swap)
        {
            if (GameState.CurrentHero.PurchaseItem(purchaseItem.Item)) // if purchasing and can afford it
            {
                if (swap)
                    SwapItems(slot, purchaseItem);
                else
                    slot.PutItemInSlot(purchaseItem);
            }
            else
                slot.LblError.Text = "You cannot afford that item.";
        }

        private void SellItem(ItemSlot slot, InventoryItem sellItem, bool swap)
        {
            if (sellItem.Item.CanSell)
            {
                GameState.CurrentHero.SellItem(sellItem.Item);
                if (swap)
                    SwapItems(slot, sellItem);
                else
                    slot.PutItemInSlot(sellItem);
            }
            else
                slot.LblError.Text = "This item cannot be sold.";
        }

        /// <summary>Swaps the Item currently in the <see cref="ItemSlot"/> with the <see cref="InventoryItem"/> from the <see cref="Orphanage"/>.</summary>
        /// <param name="slot"><see cref="ItemSlot"/> holding this <see cref="InventoryItem"/></param>
        /// <param name="orphanItem"><see cref="InventoryItem"/> in the <see cref="Orphanage"/></param>
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

        /// <summary>Sets the Item in the <see cref="InventoryItem"/>.</summary>
        /// <param name="item">Item to be set</param>
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