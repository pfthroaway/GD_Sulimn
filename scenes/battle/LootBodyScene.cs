using Godot;
using Sulimn.Classes;
using Sulimn.Scenes.Inventory;

namespace Sulimn.Scenes.Battle
{
    public class LootBodyScene : Control
    {
        private GridEquipment HeroEquipment, EnemyEquipment;
        private GridInventory HeroInventory, EnemyInventory;
        private Button BtnLootGold;
        private Label LblGold;

        #region Button Click

        private void _on_BtnLootGold_pressed()
        {
            GameState.CurrentHero.Gold += GameState.CurrentEnemy.Gold;
            GameState.CurrentEnemy.Gold = 0;
            GameState.UpdateDisplay = true;
            ToggleLootGold();
        }

        private void _on_BtnReturn_pressed() => GetTree().ChangeSceneTo(GameState.GoBack());

        #endregion Button Click

        private void ToggleLootGold()
        {
            LblGold.Text = GameState.CurrentEnemy.GoldToStringWithText;
            BtnLootGold.Disabled = GameState.CurrentEnemy.Gold == 0;
        }

        #region Save

        private void Save()
        {
            SaveInventory();
            SaveEquipment();
        }

        private void SaveInventory()
        {
            GameState.SetInventoryFromGrid(HeroInventory);
            GameState.SetInventoryFromGrid(EnemyInventory, false);
        }

        private void SaveEquipment()
        {
            GameState.SetEquipmentFromGrid(HeroEquipment);
            GameState.SetEquipmentFromGrid(EnemyEquipment, false);
        }

        #endregion Save

        private void AssignControls()
        {
            HeroInventory = (GridInventory)GetNode("HeroInventory");
            EnemyInventory = (GridInventory)GetNode("EnemyInventory");
            HeroEquipment = (GridEquipment)GetNode("HeroEquipment");
            EnemyEquipment = (GridEquipment)GetNode("EnemyEquipment");
            BtnLootGold = (Button)FindNode("BtnLootGold");
            LblGold = (Label)FindNode("LblGold");
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            AssignControls();
            HeroInventory.SetUpInventory(GameState.CurrentHero.Inventory);
            EnemyInventory.SetUpInventory(GameState.CurrentEnemy.Inventory, true);
            HeroEquipment.SetUpEquipment(GameState.CurrentHero.Equipment, GameState.CurrentHero.Level, GameState.CurrentHero.Class);
            EnemyEquipment.SetUpEquipment(GameState.CurrentEnemy.Equipment, 0, null, true);
            ToggleLootGold();
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