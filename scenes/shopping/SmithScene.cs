using Godot;
using Sulimn.Classes.Items;
using Sulimn.Scenes.Inventory;

namespace Sulimn.Classes.Shopping
{
    public class SmithScene : Control
    {
        private Button BtnRepair, BtnRepairAll;
        private GridEquipment HeroEquipment;
        private GridInventory HeroInventory;
        private ItemSlot RepairSlot;
        private Label LblRepair, LblRepairAll;
        private Orphanage orphanage;

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
                GetTree().ChangeSceneTo(GameState.GoBack());
        }

        /// <summary>Assigns all controls to something usable in code.</summary>
        private void AssignControls()
        {
            BtnRepair = (Button)GetNode("BtnRepair");
            BtnRepairAll = (Button)GetNode("BtnRepairAll");
            HeroInventory = (GridInventory)GetNode("HeroInventory");
            HeroEquipment = (GridEquipment)GetNode("HeroEquipment");
            LblRepair = (Label)GetNode("LblRepair");
            LblRepairAll = (Label)GetNode("LblRepairAll");
            orphanage = (Orphanage)GetNode("Orphanage");
            RepairSlot = (ItemSlot)GetNode("RepairSlot");

            HeroInventory.SetUpInventory(GameState.CurrentHero.Inventory);
            HeroEquipment.SetUpEquipment(GameState.CurrentHero.Equipment, GameState.CurrentHero.Level, GameState.CurrentHero.Class);
        }

        public override void _Ready()
        {
            AssignControls();
            UpdateLabels();
        }

        /// <summary>Updates all labels.</summary>
        private void UpdateLabels()
        {
            if (RepairSlot.Item.Item != null && RepairSlot.Item.Item != new Item())
            {
                LblRepair.Text = RepairSlot.Item.Item.RepairCostToStringWithText;
                BtnRepair.Disabled = RepairSlot.Item.Item.RepairCost == 0 || GameState.CurrentHero.Gold < RepairSlot.Item.Item.RepairCost;
            }
            else
            {
                LblRepair.Text = "";
                BtnRepair.Disabled = true;
            }
            LblRepairAll.Text = GameState.CurrentHero.TotalRepairCostToStringWithText;
            BtnRepairAll.Disabled = GameState.CurrentHero.TotalRepairCost == 0 || GameState.CurrentHero.Gold < GameState.CurrentHero.TotalRepairCost;
        }

        #region Save

        /// <summary>Saves the current state of the on-screen inventory and equipment.</summary>
        private void Save()
        {
            SaveInventory();
            SaveEquipment();
        }

        /// <summary>Saves the current state of the on-screen inventory.</summary>
        private void SaveInventory() => GameState.SetInventoryFromGrid(HeroInventory);

        /// <summary>Saves the current state of the on-screen equipment.</summary>
        private void SaveEquipment() => GameState.SetEquipmentFromGrid(HeroEquipment);

        #endregion Save

        #region Button Click

        private void _on_BtnReturn_pressed() => GetTree().ChangeSceneTo(GameState.GoBack());

        private void _on_BtnRepair_pressed()
        {
            InventoryItem item = RepairSlot.Item;
            GameState.CurrentHero.Gold -= item.Item.RepairCost;
            item.Item.CurrentDurability = item.Item.MaximumDurability;
            item.SetItem(item.Item);
            ItemSlot slot = HeroInventory.FindFirstEmptySlot();
            if (slot != new ItemSlot())
            {
                RepairSlot.RemoveChild(item);
                orphanage.AddChild(item);
                slot.PutItemInSlot(item);
                RepairSlot.Item = new InventoryItem();
            }
            GameState.UpdateDisplay = true;
        }

        private void _on_BtnRepairAll_pressed()
        {
            GameState.CurrentHero.Gold -= GameState.CurrentHero.TotalRepairCost;
            GameState.CurrentHero.Inventory.ForEach(itm => itm.CurrentDurability = itm.MaximumDurability);
            GameState.CurrentHero.Equipment.AllEquipment.ForEach(itm => itm.CurrentDurability = itm.MaximumDurability);
            HeroInventory.SetUpInventory(GameState.CurrentHero.Inventory);
            HeroEquipment.SetUpEquipment(GameState.CurrentHero.Equipment, GameState.CurrentHero.Level, GameState.CurrentHero.Class);
            GameState.UpdateDisplay = true;
        }

        #endregion Button Click

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            if (GameState.UpdateDisplay)
            {
                UpdateLabels();
                GameState.UpdateDisplay = false;
                GameState.Info.DisplayStats();
                Save();
            }
        }
    }
}
