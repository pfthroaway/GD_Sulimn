using Godot;
using Sulimn.Classes;
using Sulimn.Scenes.Battle;

namespace Sulimn.Scenes
{
    public class Info : CanvasLayer
    {
        private Label LblLevel, LblExperience, LblGold;
        private Button BtnCharacter;
        private TextureProgress TPHealth, TPMagic;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready() => AssignControls();

        /// <summary>Assigns all controls to local variables for easy use.</summary>
        private void AssignControls()
        {
            LblLevel = (Label)GetNode("LblLevel");
            LblExperience = (Label)GetNode("LblExperience");
            LblGold = (Label)GetNode("LblGold");
            BtnCharacter = (Button)GetNode("BtnCharacter");
            TPHealth = (TextureProgress)GetNode("TPHealth");
            TPMagic = (TextureProgress)GetNode("TPMagic");
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
            TPHealth.Value = (float)GameState.CurrentHero.Statistics.HealthRatio * 100;
            TPHealth.SetTooltip(GameState.CurrentHero.Statistics.HealthToStringWithText);
            TPMagic.Value = (float)GameState.CurrentHero.Statistics.MagicRatio * 100;
            TPMagic.SetTooltip(GameState.CurrentHero.Statistics.MagicToStringWithText);
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