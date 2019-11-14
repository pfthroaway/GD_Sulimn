using Godot;
using Sulimn.Actors;
using Sulimn.Classes;
using Sulimn.Classes.Extensions;

namespace Sulimn.Scenes.Exploration
{
    public class FieldsScene : Node2D
    {
        private Info info;
        private Player Player;
        private Vector2 PreviousPosition;
        private AcceptDialog acceptDialog;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Player = (Player)GetTree().CurrentScene.FindNode("Player");
            acceptDialog = (AcceptDialog)GetNode("MyAcceptDialog");
            info = (Info)GetNode("/root/Info");
        }

        public override void _Process(float delta)
        {
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _PhysicsProcess(float delta)
        {
            if (PreviousPosition != Player.GetGlobalPosition())
            {
                PreviousPosition = Player.GetGlobalPosition();
                CheckForEvents();
            }
        }

        private void _on_CityArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
                GetTree().ChangeSceneTo(GameState.GoBack());
        }

        private void DisplayPopup(string text)
        {
            acceptDialog.Popup_();
            acceptDialog.DialogText = text;
            acceptDialog.SetGlobalPosition(new Vector2(Player.GetGlobalPosition().x + 32, Player.GetGlobalPosition().y - 32));
        }

        #region Events

        /// <summary>Check whether the an event happened on this move.</summary>
        private void CheckForEvents()
        {
            if (GameState.CurrentHero.Statistics.CurrentHealth > 1)
            {
                if (Functions.GenerateRandomNumber(1, 100) < 10)
                {
                    GameState.AddSceneToHistory(GetTree().CurrentScene);
                    GameState.EventEncounterEnemy(1, 5);
                    GetTree().ChangeScene("res://scenes/battle/BattleScene.tscn");
                }
                else if (Functions.GenerateRandomNumber(1, 100) < 5)
                {
                    DisplayPopup(GameState.EventFindItem(1, 100));
                    info.DisplayStats();
                }
                else if (Functions.GenerateRandomNumber(1, 100) < 5)
                {
                    DisplayPopup(GameState.EventFindGold(1, 100));
                    info.DisplayStats();
                }
            }
        }

        #endregion Events
    }
}