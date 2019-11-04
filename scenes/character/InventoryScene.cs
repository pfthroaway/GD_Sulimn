using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Enums;
using Sulimn.Classes.Inventory;
using Sulimn.Classes.Items;
using System.Collections.Generic;
using System.Linq;

public class InventoryScene : Control
{
    private TextureRect WeaponRect, HeadRect, BodyRect, HandsRect, LegsRect, FeetRect, LeftRingRect, RightRingRect;
    private InventoryItem Weapon, Head, Body, Hands, Legs, Feet, LeftRing, RightRing;
    private ItemSlot WeaponSlot, HeadSlot, BodySlot, HandsSlot, LegsSlot, FeetSlot, LeftRingSlot, RightRingSlot;
    private GridContainer GridInventory;
    private List<ItemSlot> slotList = new List<ItemSlot>();
    private List<InventoryItem> itemList = new List<InventoryItem>();
    private InventoryItem holdingItem;

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
        {
            GetTree().ChangeSceneTo(GameState.PreviousScene);
            GameState.SaveHero(GameState.CurrentHero);
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AssignControls();
        SetTooltips();
        // TODO I need to figure out a way to remember my navigation so I can return to the proper scene from the Inventory Scene.
    }

    /// <summary>Assign all controls.</summary>
    private void AssignControls()
    {
        WeaponRect = (TextureRect)GetNode("Weapon");
        HeadRect = (TextureRect)GetNode("Head");
        BodyRect = (TextureRect)GetNode("Body");
        HandsRect = (TextureRect)GetNode("Hands");
        LegsRect = (TextureRect)GetNode("Legs");
        FeetRect = (TextureRect)GetNode("Feet");
        LeftRingRect = (TextureRect)GetNode("LeftRing");
        RightRingRect = (TextureRect)GetNode("RightRing");
        RightRingRect = (TextureRect)GetNode("RightRing");
        GridInventory = (GridContainer)GetNode("GridInventory");

        SetUpInventory();
        SetUpEquipment();
    }

    private void SetUpInventory()
    {
        foreach (Item item in GameState.CurrentHero.Inventory)
            itemList.Add(new InventoryItem(item, null));

        for (int i = 0; i < 40; i++)
        {
            ItemSlot slot = new ItemSlot(i);
            slotList.Add(slot);
            GridInventory.AddChild(slot);
        }

        for (int i = 0; i < itemList.Count; i++)
            slotList[i].SetItem(itemList[i]);
    }

    private void SetUpEquipment()
    {
        WeaponSlot = new ItemSlot(40);
        HeadSlot = new ItemSlot(41);
        BodySlot = new ItemSlot(42);
        HandsSlot = new ItemSlot(43);
        LegsSlot = new ItemSlot(44);
        FeetSlot = new ItemSlot(45);
        LeftRingSlot = new ItemSlot(46);
        RightRingSlot = new ItemSlot(47);
        if (GameState.CurrentHero.Equipment.Weapon != new Item())
        {
            Weapon = new InventoryItem(GameState.CurrentHero.Equipment.Weapon, WeaponSlot);
            WeaponSlot.SetItem(Weapon);
        }
        if (GameState.CurrentHero.Equipment.Head != new Item())
        {
            Head = new InventoryItem(GameState.CurrentHero.Equipment.Head, HeadSlot);
            HeadSlot.SetItem(Head);
        }
        if (GameState.CurrentHero.Equipment.Body != new Item())
        {
            Body = new InventoryItem(GameState.CurrentHero.Equipment.Body, BodySlot);
            BodySlot.SetItem(Body);
        }
        if (GameState.CurrentHero.Equipment.Hands != new Item())
        {
            Hands = new InventoryItem(GameState.CurrentHero.Equipment.Hands, HandsSlot);
            HandsSlot.SetItem(Hands);
        }
        if (GameState.CurrentHero.Equipment.Legs != new Item())
        {
            Legs = new InventoryItem(GameState.CurrentHero.Equipment.Legs, LegsSlot);
            LegsSlot.SetItem(Legs);
        }
        if (GameState.CurrentHero.Equipment.Feet != new Item())
        {
            Feet = new InventoryItem(GameState.CurrentHero.Equipment.Feet, FeetSlot);
            FeetSlot.SetItem(Feet);
        }
        if (GameState.CurrentHero.Equipment.LeftRing != new Item())
        {
            LeftRing = new InventoryItem(GameState.CurrentHero.Equipment.LeftRing, LeftRingSlot);
            LeftRingSlot.SetItem(LeftRing);
        }
        if (GameState.CurrentHero.Equipment.RightRing != new Item())
        {
            RightRing = new InventoryItem(GameState.CurrentHero.Equipment.RightRing, RightRingSlot);
            RightRingSlot.SetItem(RightRing);
        }

        slotList.Add(WeaponSlot);
        slotList.Add(HeadSlot);
        slotList.Add(BodySlot);
        slotList.Add(HandsSlot);
        slotList.Add(LegsSlot);
        slotList.Add(FeetSlot);
        slotList.Add(LeftRingSlot);
        slotList.Add(RightRingSlot);

        WeaponRect.AddChild(WeaponSlot);
        HeadRect.AddChild(HeadSlot);
        BodyRect.AddChild(BodySlot);
        HandsRect.AddChild(HandsSlot);
        LegsRect.AddChild(LegsSlot);
        FeetRect.AddChild(FeetSlot);
        LeftRingRect.AddChild(LeftRingSlot);
        RightRingRect.AddChild(RightRingSlot);
    }

    private void SetTooltips()
    {
        HeadRect.SetTooltip(GameState.CurrentHero.Equipment.Head.TooltipText);
        BodyRect.SetTooltip(GameState.CurrentHero.Equipment.Body.TooltipText);
        HandsRect.SetTooltip(GameState.CurrentHero.Equipment.Hands.TooltipText);
        LegsRect.SetTooltip(GameState.CurrentHero.Equipment.Legs.TooltipText);
        FeetRect.SetTooltip(GameState.CurrentHero.Equipment.Feet.TooltipText);
        WeaponRect.SetTooltip(GameState.CurrentHero.Equipment.Weapon.TooltipText);
        LeftRingRect.SetTooltip(GameState.CurrentHero.Equipment.LeftRing.TooltipText);
        RightRingRect.SetTooltip(GameState.CurrentHero.Equipment.RightRing.TooltipText);
    }

    #region Item Manipulation

    /// <summary>Equips the currently held <see cref="Item"/> and swaps out the previously equipped <see cref="Item"/>.</summary>
    /// <param name="clickedSlot"><see cref="ItemSlot"/> that was clicked on</param>
    /// <param name="hand"><see cref="RingHand"/> to place a Ring on if <see cref="Item"/> is a Ring</param>
    private void EquipAndSwap(ItemSlot clickedSlot, RingHand hand = RingHand.Left)
    {
        GameState.CurrentHero.Equip(holdingItem.Item, hand);
        SwapItems(clickedSlot);
    }

    /// <summary>Finds the first empty <see cref="ItemSlot"/> in the slotList.</summary>
    /// <returns>First empty <see cref="ItemSlot"/></returns>
    private ItemSlot FindFirstEmptySlot() => slotList.First(slot => slot.Item == null);

    /// <summary>Put an <see cref="InventoryItem"/> into an <see cref="ItemSlot"/>.</summary>
    /// <param name="clickedSlot"><see cref="ItemSlot"/> that was clicked on</param>
    private void PutItem(ItemSlot clickedSlot)
    {
        clickedSlot.PutItem(holdingItem);
        holdingItem = null;
    }

    /// <summary>Swap <see cref="InventoryItem"/>s between <see cref="ItemSlot"/>s.</summary>
    /// <param name="clickedSlot"><see cref="ItemSlot"/> that was clicked on</param>
    private void SwapItems(ItemSlot clickedSlot)
    {
        InventoryItem tempItem = clickedSlot.Item;
        ItemSlot oldSlot = slotList[holdingItem.Slot.SlotIndex];
        clickedSlot.PickItem();
        clickedSlot.PutItem(holdingItem);
        holdingItem = null;
        oldSlot.PutItem(tempItem);
    }

    /// <summary>Unequips the currently held <see cref="Item"/> and puts it in the Inventory.</summary>
    /// <param name="clickedSlot"><see cref="ItemSlot"/> that was clicked on</param>
    /// <param name="hand"><see cref="RingHand"/> to place a Ring on if <see cref="Item"/> is a Ring</param>
    private void UnEquipAndPut(ItemSlot clickedSlot, RingHand hand = RingHand.Left)
    {
        if (clickedSlot.SlotIndex <= 40)
        {
            GameState.CurrentHero.Unequip(holdingItem.Item, hand);
            PutItem(clickedSlot);
        }
    }

    #endregion Item Manipulation

    #region Input

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion && holdingItem?.Picked == true)
            holdingItem.RectGlobalPosition = new Vector2(mouseMotion.Position.x, mouseMotion.Position.y);
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton button && button.ButtonIndex == 1 && button.Pressed)
        {
            ItemSlot clickedSlot = null;

            // Check if the slot being clicked is in the slotList
            foreach (ItemSlot slot in slotList)
            {
                Vector2 slotMousePos = slot.GetLocalMousePosition();
                Texture slotTexture = slot.Texture;
                bool isClicked = slotMousePos.x >= 0 && slotMousePos.x <= slotTexture.GetWidth() && slotMousePos.y >= 0 && slotMousePos.y <= slotTexture.GetHeight();
                if (isClicked)
                    clickedSlot = slot;
            }

            // If currently holding an item
            if (holdingItem != null)
            {
                // If you clicked into a valid slot and the item isn't null
                if (clickedSlot?.Item != null)
                {
                    // Check to see if it is trying to go into an equipment slot.
                    if (clickedSlot.SlotIndex > 40)
                    {
                        // If the slot already has a valid piece of equipment in it,
                        // check if the item going into it is a valid piece of equipment.
                        // If it's not a valid piece of equipment, don't let it go in.
                        // If the currently held item is a valid piece of equipment,
                        // let it go into that slot.
                        switch (clickedSlot.SlotIndex)
                        {
                            case 40: // Weapon
                                if (holdingItem.Item.Type == ItemType.MeleeWeapon || holdingItem.Item.Type == ItemType.RangedWeapon)
                                    EquipAndSwap(clickedSlot);
                                break;

                            case 41: // Head
                                if (holdingItem.Item.Type == ItemType.HeadArmor)
                                    EquipAndSwap(clickedSlot);
                                break;

                            case 42: // Body
                                if (holdingItem.Item.Type == ItemType.BodyArmor)
                                    EquipAndSwap(clickedSlot);
                                break;

                            case 43: // Hands
                                if (holdingItem.Item.Type == ItemType.HandArmor)
                                    EquipAndSwap(clickedSlot);
                                break;

                            case 44: // Legs
                                if (holdingItem.Item.Type == ItemType.LegArmor)
                                    EquipAndSwap(clickedSlot);
                                break;

                            case 45: // Feet
                                if (holdingItem.Item.Type == ItemType.FeetArmor)
                                    EquipAndSwap(clickedSlot);
                                break;

                            case 46: // LeftRing
                                if (holdingItem.Item.Type == ItemType.Ring)
                                    EquipAndSwap(clickedSlot, RingHand.Left);
                                break;

                            case 47: // RightRing
                                if (holdingItem.Item.Type == ItemType.Ring)
                                    EquipAndSwap(clickedSlot, RingHand.Right);
                                break;
                        }
                    }
                    // If not going into an equipment slot
                    // check to see if the item came from an equipment slot.
                    else
                    {
                        // If it came from an equipment slot, make sure the item
                        // attempting to be swapped with is a valid piece of equipment for that slot.
                        // If it's not, unequip it and drop it in the first available slot.
                        if (holdingItem.Slot.SlotIndex > 40)
                        {
                            if (holdingItem.Item.Type == clickedSlot.Item.Item.Type)
                                EquipAndSwap(clickedSlot);
                            else if (holdingItem.Item.Type == ItemType.MeleeWeapon || holdingItem.Item.Type == ItemType.RangedWeapon)
                            {
                                if (clickedSlot.Item.Item.Type == ItemType.MeleeWeapon || clickedSlot.Item.Item.Type == ItemType.RangedWeapon)
                                    EquipAndSwap(clickedSlot);
                            }
                            else
                                UnEquipAndPut(FindFirstEmptySlot());
                        }
                        // If the item didn't come from an equipment slot, swap the items.
                        else
                            SwapItems(clickedSlot);
                    }
                }
                // If you clicked into a valid slot and there's nothing there
                else if (clickedSlot != null)
                {
                    // If attempting to equip something, check if it's in a valid place.
                    if (clickedSlot?.SlotIndex > 40)
                    {
                        switch (clickedSlot.SlotIndex)
                        {
                            case 40: // Weapon
                                if (holdingItem.Item.Type == ItemType.MeleeWeapon || holdingItem.Item.Type == ItemType.RangedWeapon)
                                {
                                    GameState.CurrentHero.Equip(holdingItem.Item);
                                    PutItem(clickedSlot);
                                }
                                break;

                            case 41: // Head
                                if (holdingItem.Item.Type == ItemType.HeadArmor)
                                {
                                    GameState.CurrentHero.Equip(holdingItem.Item);
                                    PutItem(clickedSlot);
                                }
                                break;

                            case 42: // Body
                                if (holdingItem.Item.Type == ItemType.BodyArmor)
                                {
                                    GameState.CurrentHero.Equip(holdingItem.Item);
                                    PutItem(clickedSlot);
                                }
                                break;

                            case 43: // Hands
                                if (holdingItem.Item.Type == ItemType.HandArmor)
                                {
                                    GameState.CurrentHero.Equip(holdingItem.Item);
                                    PutItem(clickedSlot);
                                }
                                break;

                            case 44: // Legs
                                if (holdingItem.Item.Type == ItemType.LegArmor)
                                {
                                    GameState.CurrentHero.Equip(holdingItem.Item);
                                    PutItem(clickedSlot);
                                }
                                break;

                            case 45: // Feet
                                if (holdingItem.Item.Type == ItemType.FeetArmor)
                                {
                                    GameState.CurrentHero.Equip(holdingItem.Item);
                                    PutItem(clickedSlot);
                                }
                                break;

                            case 46: // LeftRing
                                if (holdingItem.Item.Type == ItemType.Ring)
                                {
                                    GameState.CurrentHero.Equip(holdingItem.Item, RingHand.Left);
                                    PutItem(clickedSlot);
                                }
                                break;

                            case 47: // RightRing
                                if (holdingItem.Item.Type == ItemType.Ring)
                                {
                                    GameState.CurrentHero.Equip(holdingItem.Item, RingHand.Right);
                                    PutItem(clickedSlot);
                                }
                                break;
                        }
                    }
                    else if (holdingItem.Slot.SlotIndex > 40)
                        UnEquipAndPut(clickedSlot);
                    else
                        PutItem(clickedSlot);
                }
            }
            else if (clickedSlot?.Item != null)
            {
                holdingItem = clickedSlot.Item;
                clickedSlot.PickItem();
                holdingItem.RectGlobalPosition = new Vector2(button.Position.x, button.Position.y);
            }
        }
    }

    #endregion Input

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}