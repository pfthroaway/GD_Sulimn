using Godot;
using Sulimn.Classes;
using Sulimn.Scenes.Inventory;
using System.Linq;

public class LootBodyScene : Control
{
    private GridEquipment HeroEquipment, EnemyEquipment;
    private GridInventory HeroInventory, EnemyInventory;

    private void _on_BtnReturn_pressed() => GetTree().ChangeSceneTo(GameState.GoBack());

    #region Save

    private void Save()
    {
        SaveInventory();
        SaveEquipment();
    }

    private void SaveInventory() => GameState.SetInventoryFromGrid(HeroInventory);

    private void SaveEquipment() => GameState.SetEquipmentFromGrid(HeroEquipment);

    #endregion Save

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        HeroInventory = (GridInventory)GetNode("HeroInventory");
        EnemyInventory = (GridInventory)GetNode("EnemyInventory");
        HeroEquipment = (GridEquipment)GetNode("HeroEquipment");
        EnemyEquipment = (GridEquipment)GetNode("EnemyEquipment");
        HeroInventory.SetUpInventory(GameState.CurrentHero.Inventory);
        EnemyInventory.SetUpInventory(GameState.CurrentEnemy.Inventory.Where(itm => itm.IsLootable).ToList());
        HeroEquipment.SetUpEquipment(GameState.CurrentHero.Equipment);
        EnemyEquipment.SetUpEquipment(GameState.CurrentEnemy.Equipment, true);
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