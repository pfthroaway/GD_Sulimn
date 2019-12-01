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
                if (button.ButtonIndex == 1 && !Input.IsKeyPressed((int)KeyList.Control) && !Input.IsKeyPressed((int)KeyList.Shift))
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
                    slot.LblError.Text = "";
                    if (GetTree().CurrentScene.Name == "ItemMerchant")
                    {
                        Node container = GetParent().GetParent().GetParent();
                        if (container.Name == "HeroInventory")
                        {
                            MerchantInventory merchantInventory = (MerchantInventory)GetTree().CurrentScene.FindNode("MerchantInventory");
                            if (merchantInventory.GetItemsInInventory() < 80 && Item.CanSell)
                            {
                                PutItemInOrphanage(slot);
                                SellItem(merchantInventory.FindFirstEmptySlot(), this, false);
                            }
                            else if (merchantInventory.GetItemsInInventory() >= 80)
                                slot.LblError.Text = "The merchant's inventory is full.";
                            else if (!Item.CanSell)
                                slot.LblError.Text = "This item cannot be sold.";
                        }
                        else if (container.Name == "MerchantInventory")
                        {
                            GridInventory heroInventory = (GridInventory)GetTree().CurrentScene.FindNode("HeroInventory");
                            if (GameState.CurrentHero.Inventory.Count < 40 && GameState.CurrentHero.Gold >= Item.Value)
                            {
                                PutItemInOrphanage(slot);
                                PurchaseItem(heroInventory.FindFirstEmptySlot(), this, false);
                            }
                            else if (GameState.CurrentHero.Inventory.Count >= 40)
                                slot.LblError.Text = "Your inventory is full.";
                            else if (GameState.CurrentHero.Gold < Item.Value)
                                slot.LblError.Text = "You cannot afford that item.";
                        }
                    }
                    else if (GetTree().CurrentScene.Name == "LootBody")
                    {
                        Node container = GetParent().GetParent().GetParent();
                        if (container.Name == "HeroInventory")
                        {
                            GridInventory enemyInventory = (GridInventory)GetTree().CurrentScene.FindNode("EnemyInventory");

                            if (GameState.CurrentEnemy.Inventory.Count < 40)
                            {
                                PutItemInOrphanage(slot);
                                enemyInventory.FindFirstEmptySlot().PutItemInSlot(this);
                            }
                            else if (GameState.CurrentEnemy.Inventory.Count >= 40)
                                slot.LblError.Text = $"The {GameState.CurrentEnemy.Name}'s inventory is full.";
                        }
                        else if (container.Name == "EnemyInventory")
                        {
                            GridInventory heroInventory = (GridInventory)GetTree().CurrentScene.FindNode("HeroInventory");
                            if (GameState.CurrentHero.Inventory.Count < 40)
                            {
                                PutItemInOrphanage(slot);
                                heroInventory.FindFirstEmptySlot().PutItemInSlot(this);
                            }
                            else if (GameState.CurrentHero.Inventory.Count >= 40)
                                slot.LblError.Text = "Your inventory is full.";
                        }
                    }
                }
                else if (button.ButtonIndex == 1 && Input.IsKeyPressed((int)KeyList.Shift))
                {
                    slot.LblError.Text = "";
                    string scene = GetTree().CurrentScene.Name;
                    if (scene == "ItemMerchant" || scene == "LootBody" || scene == "CharacterScene")
                    {
                        Node container = GetParent().GetParent().GetParent();
                        if (container.Name == "HeroInventory")
                        {
                            GridEquipment heroEquipment = (GridEquipment)GetTree().CurrentScene.FindNode("HeroEquipment");
                            switch (Item.Type)
                            {
                                case ItemType.MeleeWeapon:
                                case ItemType.RangedWeapon:
                                    if (heroEquipment.WeaponSlot.Item.Item != new Item() && CheckItemLevel(heroEquipment.WeaponSlot, this) && CheckItemClasses(heroEquipment.WeaponSlot, this))
                                        SwapSlotContents(slot, heroEquipment.WeaponSlot);
                                    else if (heroEquipment.WeaponSlot.Item.Item == new Item() && CheckItemLevel(heroEquipment.WeaponSlot, this) && CheckItemClasses(heroEquipment.WeaponSlot, this))
                                    {
                                        PutItemInOrphanage(slot);
                                        heroEquipment.WeaponSlot.PutItemInSlot(orphanage.GetItem());
                                    }
                                    else if (!CheckItemLevel(heroEquipment.WeaponSlot, this))
                                        slot.LblError.Text = "You are not high enough level to equip this item.";
                                    else if (!CheckItemClasses(heroEquipment.WeaponSlot, this))
                                        slot.LblError.Text = "You are unable to equip this item because you are not the correct class.";
                                    break;

                                case ItemType.HeadArmor:
                                    if (heroEquipment.HeadSlot.Item.Item != new Item() && CheckItemLevel(heroEquipment.HeadSlot, this) && CheckItemClasses(heroEquipment.HeadSlot, this))
                                        SwapSlotContents(slot, heroEquipment.HeadSlot);
                                    else if (heroEquipment.HeadSlot.Item.Item == new Item() && CheckItemLevel(heroEquipment.HeadSlot, this) && CheckItemClasses(heroEquipment.HeadSlot, this))
                                    {
                                        PutItemInOrphanage(slot);
                                        heroEquipment.HeadSlot.PutItemInSlot(orphanage.GetItem());
                                    }
                                    else if (!CheckItemLevel(heroEquipment.HeadSlot, this))
                                        slot.LblError.Text = "You are not high enough level to equip this item.";
                                    else if (!CheckItemClasses(heroEquipment.HeadSlot, this))
                                        slot.LblError.Text = "You are unable to equip this item because you are not the correct class.";
                                    break;

                                case ItemType.BodyArmor:
                                    if (heroEquipment.BodySlot.Item.Item != new Item() && CheckItemLevel(heroEquipment.BodySlot, this) && CheckItemClasses(heroEquipment.BodySlot, this))
                                        SwapSlotContents(slot, heroEquipment.BodySlot);
                                    else if (heroEquipment.BodySlot.Item.Item == new Item() && CheckItemLevel(heroEquipment.BodySlot, this) && CheckItemClasses(heroEquipment.BodySlot, this))
                                    {
                                        PutItemInOrphanage(slot);
                                        heroEquipment.BodySlot.PutItemInSlot(orphanage.GetItem());
                                    }
                                    else if (!CheckItemLevel(heroEquipment.BodySlot, this))
                                        slot.LblError.Text = "You are not high enough level to equip this item.";
                                    else if (!CheckItemClasses(heroEquipment.BodySlot, this))
                                        slot.LblError.Text = "You are unable to equip this item because you are not the correct class.";
                                    break;

                                case ItemType.HandArmor:
                                    if (heroEquipment.HandsSlot.Item.Item != new Item() && CheckItemLevel(heroEquipment.HandsSlot, this) && CheckItemClasses(heroEquipment.HandsSlot, this))
                                        SwapSlotContents(slot, heroEquipment.HandsSlot);
                                    else if (heroEquipment.HandsSlot.Item.Item == new Item() && CheckItemLevel(heroEquipment.HandsSlot, this) && CheckItemClasses(heroEquipment.HandsSlot, this))
                                    {
                                        PutItemInOrphanage(slot);
                                        heroEquipment.HandsSlot.PutItemInSlot(orphanage.GetItem());
                                    }
                                    else if (!CheckItemLevel(heroEquipment.HandsSlot, this))
                                        slot.LblError.Text = "You are not high enough level to equip this item.";
                                    else if (!CheckItemClasses(heroEquipment.HandsSlot, this))
                                        slot.LblError.Text = "You are unable to equip this item because you are not the correct class.";
                                    break;

                                case ItemType.LegArmor:
                                    if (heroEquipment.LegsSlot.Item.Item != new Item() && CheckItemLevel(heroEquipment.LegsSlot, this) && CheckItemClasses(heroEquipment.LegsSlot, this))
                                        SwapSlotContents(slot, heroEquipment.LegsSlot);
                                    else if (heroEquipment.LegsSlot.Item.Item == new Item() && CheckItemLevel(heroEquipment.LegsSlot, this) && CheckItemClasses(heroEquipment.LegsSlot, this))
                                    {
                                        PutItemInOrphanage(slot);
                                        heroEquipment.LegsSlot.PutItemInSlot(orphanage.GetItem());
                                    }
                                    else if (!CheckItemLevel(heroEquipment.LegsSlot, this))
                                        slot.LblError.Text = "You are not high enough level to equip this item.";
                                    else if (!CheckItemClasses(heroEquipment.LegsSlot, this))
                                        slot.LblError.Text = "You are unable to equip this item because you are not the correct class.";
                                    break;

                                case ItemType.FeetArmor:
                                    if (heroEquipment.FeetSlot.Item.Item != new Item() && CheckItemLevel(heroEquipment.FeetSlot, this) && CheckItemClasses(heroEquipment.FeetSlot, this))
                                        SwapSlotContents(slot, heroEquipment.FeetSlot);
                                    else if (heroEquipment.FeetSlot.Item.Item == new Item() && CheckItemLevel(heroEquipment.FeetSlot, this) && CheckItemClasses(heroEquipment.FeetSlot, this))
                                    {
                                        PutItemInOrphanage(slot);
                                        heroEquipment.FeetSlot.PutItemInSlot(orphanage.GetItem());
                                    }
                                    else if (!CheckItemLevel(heroEquipment.FeetSlot, this))
                                        slot.LblError.Text = "You are not high enough level to equip this item.";
                                    else if (!CheckItemClasses(heroEquipment.FeetSlot, this))
                                        slot.LblError.Text = "You are unable to equip this item because you are not the correct class.";
                                    break;

                                case ItemType.Ring:
                                    if (heroEquipment.LeftRingSlot.Item.Item == new Item() && CheckItemLevel(heroEquipment.LeftRingSlot, this) && CheckItemClasses(heroEquipment.LeftRingSlot, this))
                                    {
                                        PutItemInOrphanage(slot);
                                        heroEquipment.LeftRingSlot.PutItemInSlot(orphanage.GetItem());
                                    }
                                    else if (heroEquipment.RightRingSlot.Item.Item == new Item() && CheckItemLevel(heroEquipment.RightRingSlot, this) && CheckItemClasses(heroEquipment.RightRingSlot, this))
                                    {
                                        PutItemInOrphanage(slot);
                                        heroEquipment.RightRingSlot.PutItemInSlot(orphanage.GetItem());
                                    }
                                    else if (heroEquipment.LeftRingSlot.Item.Item != new Item() && CheckItemLevel(heroEquipment.LeftRingSlot, this) && CheckItemClasses(heroEquipment.LeftRingSlot, this))
                                        SwapSlotContents(slot, heroEquipment.LeftRingSlot);
                                    else if (!CheckItemLevel(heroEquipment.LeftRingSlot, this))
                                        slot.LblError.Text = "You are not high enough level to equip this item.";
                                    else if (!CheckItemClasses(heroEquipment.LeftRingSlot, this))
                                        slot.LblError.Text = "You are unable to equip this item because you are not the correct class.";
                                    break;
                            }
                        }
                        else if (GetParent().GetParent().Name == "HeroEquipment")
                        {
                            GridInventory heroInventory = (GridInventory)GetTree().CurrentScene.FindNode("HeroInventory");
                            if (GameState.CurrentHero.Inventory.Count < 40)
                            {
                                PutItemInOrphanage(slot);
                                heroInventory.FindFirstEmptySlot().PutItemInSlot(this);
                            }
                            else if (GameState.CurrentHero.Inventory.Count >= 40)
                                slot.LblError.Text = "Your inventory is full.";
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

        private void SwapSlotContents(ItemSlot thisSlot, ItemSlot swapSlot)
        {
            Item tempItem = new Item(swapSlot.Item.Item);
            swapSlot.Item.SetItem(Item);
            thisSlot.Item.SetItem(tempItem);

            GameState.UpdateDisplay = true;
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