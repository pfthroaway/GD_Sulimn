using Godot;
using Sulimn.Classes;
using Sulimn.Scenes.Battle;

namespace Sulimn.Scenes
{
    public class Info : CanvasLayer
    {
        private Label LblLevel, LblExperience, LblGold, LblHealth, LblMagic;
        private Button BtnCharacter;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready() => AssignControls();

        /// <summary>Assigns all controls to local variables for easy use.</summary>
        private void AssignControls()
        {
            LblHealth = (Label)GetNode("LblHealth");
            LblMagic = (Label)GetNode("LblMagic");
            LblLevel = (Label)GetNode("LblLevel");
            LblExperience = (Label)GetNode("LblExperience");
            LblGold = (Label)GetNode("LblGold");
            BtnCharacter = (Button)GetNode("BtnCharacter");
        }

        private void _on_BtnCharacter_pressed()
        {
            if (GetTree().CurrentScene.Name == "CharacterScene")
                GetTree().ChangeSceneTo(GameState.GoBack());
            else
            {
                if (GetTree().CurrentScene.Name == "BattleScene")
                    GameState.BattleScene = (BattleScene)GetTree().CurrentScene;
                GameState.AddSceneToHistory(GetTree().CurrentScene);
                GetTree().ChangeScene("res://scenes/character/CharacterScene.tscn");
            }
        }

        /// <summary>Updates labels to current values.</summary>
        public void DisplayStats()
        {
            LblHealth.Text = GameState.CurrentHero.Statistics.HealthToStringWithText;
            LblMagic.Text = GameState.CurrentHero.Statistics.MagicToStringWithText;
            LblLevel.Text = GameState.CurrentHero.LevelAndClassToString;
            LblExperience.Text = GameState.CurrentHero.ExperienceToStringWithText;
            LblGold.Text = GameState.CurrentHero.GoldToStringWithText;
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //
        //  }
    }
}