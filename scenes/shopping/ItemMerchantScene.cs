using Godot;
using Sulimn.Classes;
using Sulimn.Scenes.Inventory;
using System.Linq;

namespace Sulimn.Scenes.Shopping
{
    public class ItemMerchantScene : Control
    {
        private GridEquipment GridEquipment;
        private GridInventory GridInventory;
        private MerchantInventory MerchantInventory;

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

        private void SaveInventory() => GameState.SetInventoryFromGrid(GridInventory);

        private void SaveEquipment() => GameState.SetEquipmentFromGrid(GridEquipment);

        /// <summary>Assigns all controls to something usable in code.</summary>
        private void AssignControls()
        {
            GridInventory = (GridInventory)GetNode("GridInventory");
            MerchantInventory = (MerchantInventory)GetNode("MerchantInventory");
            GridEquipment = (GridEquipment)GetNode("GridEquipment");
            GridInventory.SetUpInventory(GameState.CurrentHero.Inventory.Where(itm => itm.CanSell).ToList());
            GridEquipment.SetUpEquipment(GameState.CurrentHero.Equipment, GameState.CurrentHero.Level, GameState.CurrentHero.Class);
            MerchantInventory.SetUpInventory(GameState.MerchantInventory.Where(itm => itm.IsSold).ToList());
        }

        private void _on_BtnReturn_pressed() => GetTree().ChangeSceneTo(GameState.GoBack());

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