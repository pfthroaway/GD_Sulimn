using Godot;
using Sulimn.Classes;
using Sulimn.Scenes.Battle;

namespace Sulimn.Scenes
{
    public class Info : CanvasLayer
    {
        private Button BtnCharacter, BtnHelp, BtnSettings;
        private Label LblLevel, LblExperience, LblGold, LblText;
        private TextureProgress TPHealth, TPMagic;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready() => AssignControls();

        /// <summary>Assigns all controls to local variables for easy use.</summary>
        private void AssignControls()
        {
            BtnCharacter = (Button)FindNode("BtnCharacter");
            BtnSettings = (Button)FindNode("BtnSettings");
            BtnHelp = (Button)FindNode("BtnHelp");
            LblLevel = (Label)GetNode("LblLevel");
            LblExperience = (Label)GetNode("LblExperience");
            LblGold = (Label)GetNode("LblGold");
            LblText = (Label)GetNode("LblText");
            TPHealth = (TextureProgress)GetNode("TPHealth");
            TPMagic = (TextureProgress)GetNode("TPMagic");
        }

        /// <summary>Toggles all the Buttons on the scene.</summary>
        /// <param name="disabled">Should the buttons be disabled?</param>
        public void ToggleButtons(bool disabled)
        {
            BtnCharacter.Disabled = disabled;
            BtnHelp.Disabled = disabled;
            BtnSettings.Disabled = disabled;
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
            TPHealth.HintTooltip = GameState.CurrentHero.Statistics.HealthToStringWithText;
            TPMagic.Value = (float)GameState.CurrentHero.Statistics.MagicRatio * 100;
            TPMagic.HintTooltip = GameState.CurrentHero.Statistics.MagicToStringWithText;
            LblLevel.Text = GameState.CurrentHero.LevelAndClassToString;
            LblExperience.Text = GameState.CurrentHero.ExperienceToStringWithText;
            LblGold.Text = GameState.CurrentHero.GoldToStringWithText;
        }

        #region Mouse Enter/Exit

        private void _on_TPHealth_mouse_entered()
        {
            LblText.Text = GameState.CurrentHero.Statistics.HealthToStringWithText;
            LblText.RectGlobalPosition = new Vector2(TPHealth.RectGlobalPosition.x, TPHealth.RectGlobalPosition.y - 5);
        }

        private void _on_TPHealth_mouse_exited() => LblText.Text = "";

        private void _on_TPMagic_mouse_entered()
        {
            LblText.Text = GameState.CurrentHero.Statistics.MagicToStringWithText;
            LblText.RectGlobalPosition = new Vector2(TPMagic.RectGlobalPosition.x, TPMagic.RectGlobalPosition.y - 5);
        }

        private void _on_TPMagic_mouse_exited() => LblText.Text = "";

        #endregion Mouse Enter/Exit
    }
}
