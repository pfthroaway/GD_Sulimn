using Godot;
using Sulimn.Classes;
using Sulimn.Scenes.Inventory;
using System;

namespace Sulimn.Scenes.Shopping
{
    public class WeaponRUsScene : Control
    {
        private GridEquipment GridEquipment;
        private GridInventory GridInventory, MerchantInventory;

        // TODO Make it so that you can purchase/sell equipment, and it updates your equipment, inventory and gold properly.

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
            GridInventory.SetUpInventory(GameState.CurrentHero.Inventory, false);
            MerchantInventory.SetUpInventory(GameState.CurrentHero.Inventory, true);
            GridEquipment.SetUpEquipment(GameState.CurrentHero.Equipment);
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //
        //  }
    }
}