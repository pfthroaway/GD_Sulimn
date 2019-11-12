using Godot;
using Sulimn.Classes;
using Sulimn.Scenes.Inventory;
using System;
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
            if (GameState.UpdateEquipment)
            {
                GameState.UpdateEquipment = false;
                GameState.Info.DisplayStats();
            }
        }
    }
}