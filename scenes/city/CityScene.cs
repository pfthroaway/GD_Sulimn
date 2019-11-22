using Godot;
using Sulimn.Actors;
using Sulimn.Classes;
using Sulimn.Classes.Entities;
using Sulimn.Scenes.Exploration;

namespace Sulimn.Scenes.City
{
    public class CityScene : Node2D
    {
        private Player Player;
        private MyAcceptDialog acceptDialog;

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
            {
                GameState.SaveHero(GameState.CurrentHero);
                GetTree().ChangeScene("scenes/MainScene.tscn");
                GameState.CurrentHero = new Hero();
                GameState.History.Clear();
            }
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Player = (Player)GetTree().CurrentScene.FindNode("Player");
            acceptDialog = (MyAcceptDialog)GetNode("MyAcceptDialog");
            GameState.Info.Scale = new Vector2(1, 1);
            GameState.Info.DisplayStats();
        }

        /// <summary>Displays a popup next to the Player.</summary>
        /// <param name="text">Text to be displayed</param>
        /// <param name="displayTime">Duration for the text to be displayed</param>
        private void DisplayPopup(string text, float displayTime = 1f) => GameState.DisplayPopup(acceptDialog, Player, text, displayTime);

        #region Areas Entered

        private void _on_BankArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
            {
                Player.Move("down");
                GameState.AddSceneToHistory(GetTree().CurrentScene);
                GetTree().ChangeScene("res://scenes/city/BankScene.tscn");
            }
        }

        private void _on_CathedralArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
                Player.Move("down");
        }

        private void _on_ChapelArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
            {
                string text = "";
                if (GameState.CurrentHero.Statistics.HealthRatio <= 0.25m)
                {
                    text = "You enter a local chapel and\napproach the altar. A priest approaches you.\n" +
                "\"Let me heal your wounds. You look like\nyou've been through a tough battle.\"\n" +
                "The priest gives you a potion which heals\nyou to full health!\n" +
                "You thank the priest and return to then\nstreets.";
                    GameState.CurrentHero.Statistics.CurrentHealth = GameState.CurrentHero.Statistics.MaximumHealth;

                    GameState.SaveHero(GameState.CurrentHero);
                }
                else
                {
                    text = "You enter a local chapel. A priest\napproaches you.\n" +
                    "\"You look healthy to me. If you ever need\nhealing, don't hesitate to come see me.\"\n\n" +
                    "You thank the priest and return to the\nstreets.";
                }
                DisplayPopup(text, 3f);
                Player.Move("up");
                GameState.Info.DisplayStats();
            }
        }

        private void _on_FieldsArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
            {
                Player.Move("left");
                GameState.AddSceneToHistory(GetTree().CurrentScene);
                GetTree().ChangeScene("res://scenes/exploration/FieldsScene.tscn");
            }
        }

        private void _on_ForestArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
            {
                Player.Move("right");
                GameState.AddSceneToHistory(GetTree().CurrentScene);
                GetTree().ChangeScene("res://scenes/exploration/ForestScene.tscn");
            }
        }

        private void _on_MarketArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
            {
                Player.Move("down");
                GameState.AddSceneToHistory(GetTree().CurrentScene);
                GetTree().ChangeScene("res://scenes/shopping/MarketScene.tscn");
            }
        }

        private void _on_MinesArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
                Player.Move("up");
        }

        private void _on_TrainingArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
                Player.Move("up");
        }

        #endregion Areas Entered

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        //public override void _PhysicsProcess(float delta)
        //{
        //}
    }
}