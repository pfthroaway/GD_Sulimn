using Godot;
using Sulimn.Classes;
using Sulimn.Scenes.Inventory;

namespace Sulimn.Scenes.Battle
{
    public class EnemyDetailsScene : Control
    {
        private Label LblName, LblLevel, LblExperience, LblGold, LblStrength, LblVitality, LblDexterity, LblWisdom, LblHealth, LblMagic;
        private GridEquipment GridEquipment;
        private GridInventory GridInventory;

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
                GetTree().ChangeSceneTo(GameState.GoBack());
        }

        /// <summary>Assigns all controls to something usable in code.</summary>
        private void AssignControls()
        {
            GridInventory = (GridInventory)GetNode("GridInventory");
            GridEquipment = (GridEquipment)GetNode("GridEquipment");

            LblName = (Label)GetNode("Info/LblName");
            LblLevel = (Label)GetNode("Info/LblLevel");
            LblExperience = (Label)GetNode("Info/LblExperience");

            LblStrength = (Label)GetNode("Info/LblStrength");
            LblVitality = (Label)GetNode("Info/LblVitality");
            LblDexterity = (Label)GetNode("Info/LblDexterity");
            LblWisdom = (Label)GetNode("Info/LblWisdom");

            LblHealth = (Label)GetNode("Info/LblHealth");
            LblMagic = (Label)GetNode("Info/LblMagic");

            LblGold = (Label)GetNode("Info/LblGold");
            GridInventory.SetUpInventory(GameState.CurrentEnemy.Inventory, true);
            GridEquipment.SetUpEquipment(GameState.CurrentEnemy.Equipment, true);
        }

        /// <summary>Updates all the labels.</summary>
        public void UpdateLabels()
        {
            LblName.Text = GameState.CurrentEnemy.Name;
            LblLevel.Text = GameState.CurrentEnemy.LevelToString;
            LblExperience.Text = GameState.CurrentEnemy.ExperienceToStringWithText;
            LblStrength.Text = GameState.CurrentEnemy.Attributes.StrengthToStringWithText;
            LblVitality.Text = GameState.CurrentEnemy.Attributes.VitalityToStringWithText;
            LblDexterity.Text = GameState.CurrentEnemy.Attributes.DexterityToStringWithText;
            LblWisdom.Text = GameState.CurrentEnemy.Attributes.WisdomToStringWithText;
            LblHealth.Text = GameState.CurrentEnemy.Statistics.HealthToStringWithText;
            LblMagic.Text = GameState.CurrentEnemy.Statistics.MagicToStringWithText;
            LblGold.Text = GameState.CurrentEnemy.GoldToStringWithText;
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            AssignControls();
            UpdateLabels();
        }

        private void _on_BtnReturn_pressed() => GetTree().ChangeSceneTo(GameState.GoBack());

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //
        //  }
    }
}