using Godot;
using Sulimn.Classes;
using Sulimn.Scenes.Inventory;
using System.Linq;

namespace Sulimn.Scenes.Shopping
{
    public class WeaponsRUsScene : Control
    {
        private GridEquipment GridEquipment;
        private GridInventory GridInventory;
        private MerchantInventory MerchantInventory;

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
            MerchantInventory.SetUpInventory(GameState.AllWeapons.Where(wpn => wpn.IsSold).ToList());
            GridEquipment.SetUpEquipment(GameState.CurrentHero.Equipment);
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