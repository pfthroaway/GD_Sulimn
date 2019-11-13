using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Items;
using Sulimn.Scenes.Inventory;
using System.Collections.Generic;
using System.Linq;

namespace Sulimn.Scenes.Shopping
{
    public class WeaponsRUsScene : Control
    {
        private GridEquipment GridEquipment;
        private GridInventory GridInventory, MerchantInventory;

        // TODO Make it so that you can purchase/sell equipment, and it updates your equipment, inventory and gold properly.

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
                GetTree().ChangeSceneTo(GameState.GoBack());
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            AssignControls();
        }

        private void Save()
        {
            SaveInventory();
            SaveEquipment();
        }

        private void SaveInventory()
        {
            GridInventory = (GridInventory)GetNode("GridInventory");
            Godot.Collections.Array allSlots = GridInventory.GetChild(0).GetChildren();
            List<Item> allItems = new List<Item>();
            foreach (ItemSlot slot in allSlots)
            {
                if (slot?.Item?.Item != new Item())
                    allItems.Add(slot?.Item?.Item);
            }
            GameState.CurrentHero.Inventory = allItems;
        }

        private void SaveEquipment()
        {
            ItemSlot WeaponSlot = (ItemSlot)GridEquipment.GetNode("WeaponSlot");
            ItemSlot HeadSlot = (ItemSlot)GridEquipment.GetNode("HeadSlot");
            ItemSlot BodySlot = (ItemSlot)GridEquipment.GetNode("BodySlot");
            ItemSlot HandsSlot = (ItemSlot)GridEquipment.GetNode("HandsSlot");
            ItemSlot LegsSlot = (ItemSlot)GridEquipment.GetNode("LegsSlot");
            ItemSlot FeetSlot = (ItemSlot)GridEquipment.GetNode("FeetSlot");
            ItemSlot LeftRingSlot = (ItemSlot)GridEquipment.GetNode("LeftRingSlot");
            ItemSlot RightRingSlot = (ItemSlot)GridEquipment.GetNode("RightRingSlot");
            if (WeaponSlot.Item != null)
                GameState.CurrentHero.Equipment.Weapon = WeaponSlot.Item.Item;
            if (HeadSlot.Item != null)
                GameState.CurrentHero.Equipment.Head = HeadSlot.Item.Item;
            if (BodySlot.Item != null)
                GameState.CurrentHero.Equipment.Body = BodySlot.Item.Item;
            if (HandsSlot.Item != null)
                GameState.CurrentHero.Equipment.Hands = HandsSlot.Item.Item;
            if (LegsSlot.Item != null)
                GameState.CurrentHero.Equipment.Legs = LegsSlot.Item.Item;
            if (FeetSlot.Item != null)
                GameState.CurrentHero.Equipment.Feet = FeetSlot.Item.Item;
            if (LeftRingSlot.Item != null)
                GameState.CurrentHero.Equipment.LeftRing = LeftRingSlot.Item.Item;
            if (RightRingSlot.Item != null)
                GameState.CurrentHero.Equipment.RightRing = RightRingSlot.Item.Item;
        }

        /// <summary>Assigns all controls to something usable in code.</summary>
        private void AssignControls()
        {
            GridInventory = (GridInventory)GetNode("GridInventory");
            MerchantInventory = (GridInventory)GetNode("MerchantInventory");
            GridEquipment = (GridEquipment)GetNode("GridEquipment");
            GridInventory.SetUpInventory(GameState.CurrentHero.Inventory.Where(itm => itm.CanSell).ToList(), false);
            MerchantInventory.SetUpInventory(GameState.AllWeapons.Where(wpn => wpn.IsSold).ToList(), true);
            GridEquipment.SetUpEquipment(GameState.CurrentHero.Equipment);
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            if (GameState.UpdateDisplay)
            {
                Save();
                GameState.Info.DisplayStats();
                GameState.UpdateDisplay = false;
            }
        }
    }
}