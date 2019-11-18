using Godot;
using Sulimn.Classes.Items;
using Sulimn.Scenes.Inventory;
using System;

namespace Sulimn.Classes.Shopping
{
    public class SmithScene : Control
    {
        private Button BtnRepair, BtnRepairAll;
        private GridEquipment GridEquipment;
        private GridInventory GridInventory;
        private ItemSlot RepairSlot;
        private Label LblRepair, LblRepairAll;

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
            GridInventory = (GridInventory)GetNode("GridInventory");
            GridEquipment = (GridEquipment)GetNode("GridEquipment");
            LblRepair = (Label)GetNode("LblRepair");
            LblRepairAll = (Label)GetNode("LblRepairAll");
            RepairSlot = (ItemSlot)GetNode("RepairSlot");

            GridInventory.SetUpInventory(GameState.CurrentHero.Inventory);
            GridEquipment.SetUpEquipment(GameState.CurrentHero.Equipment);
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
                BtnRepair.Disabled = GameState.CurrentHero.Gold < RepairSlot.Item.Item.RepairCost;
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

        private void Save()
        {
            SaveInventory();
            SaveEquipment();
        }

        private void SaveInventory() => GameState.SetInventoryFromGrid(GridInventory);

        private void SaveEquipment() => GameState.SetEquipmentFromGrid(GridEquipment);

        #endregion Save

        #region Button Click

        private void _on_BtnReturn_pressed()
        {
            GameState.CurrentHero.Gold -= RepairSlot.Item.Item.RepairCost;
            RepairSlot.Item.Item.CurrentDurability = RepairSlot.Item.Item.MaximumDurability;
            // TODO Send item back to first empty inventory slot.
        }

        private void _on_BtnRepair_pressed()
        {
            // Replace with function body.
        }

        private void _on_BtnRepairAll_pressed()
        {
            // Replace with function body.
        }

        #endregion Button Click

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            if (GameState.UpdateDisplay)
            {
                UpdateLabels();
                GameState.UpdateDisplay = false;
            }
        }
    }
}