using Godot;
using Sulimn.Actors;
using Sulimn.Classes;
using Sulimn.Classes.Extensions;

namespace Sulimn.Scenes.Exploration
{
    public class FieldsScene : Node2D
    {
        private Player Player;
        private Vector2 PreviousPosition;
        private MyAcceptDialog acceptDialog;
        private int MovesSinceLastEvent;
        private int BonusChance;
        private Area2D CottageArea;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Player = (Player)GetTree().CurrentScene.FindNode("Player");
            acceptDialog = (MyAcceptDialog)GetNode("MyAcceptDialog");
            PreviousPosition = Player.GetGlobalPosition();
            CottageArea = (Area2D)GetNode("CottageArea");
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
                MovesSinceLastEvent++;
                CheckForEvents();
            }
        }

        private void _on_CityArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
                GetTree().ChangeSceneTo(GameState.GoBack());
        }

        private void _on_CottageArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
            {
                BonusChance = 50;
                CheckForEvents();
            }
        }

        private void DisplayPopup(string text, float displayTime = 1f)
        {
            acceptDialog.Popup_();
            acceptDialog.SetExpiration(displayTime);
            acceptDialog.DialogText = text;
            acceptDialog.SetGlobalPosition(new Vector2(Player.GetGlobalPosition().x + 32, Player.GetGlobalPosition().y - 32));
            GameState.Info.DisplayStats();
            MovesSinceLastEvent = 0;
        }

        #region Events

        /// <summary>Check whether the an event happened on this move.</summary>
        private void CheckForEvents()
        {
            // 10% (plus bonus) chance for an event per move starting at 1
            if (MovesSinceLastEvent > 0 && Functions.GenerateRandomNumber(1, 100) <= ((MovesSinceLastEvent * 10) + BonusChance))
                ChooseEvent();
        }

        /// <summary>Chooses which event to occur.</summary>
        private void ChooseEvent()
        {
            if (GameState.CurrentHero.Statistics.CurrentHealth > 1)
            {
                int evnt = Functions.GenerateRandomNumber(1, 20);
                if (evnt <= 4) // 20% chance for battle
                {
                    GameState.AddSceneToHistory(GetTree().CurrentScene);
                    GameState.EventEncounterEnemy(1, 5);
                    GetTree().ChangeScene("res://scenes/battle/BattleScene.tscn");
                    MovesSinceLastEvent = 0;
                }
                else if (evnt <= 6) // 10% chance to find an item
                    DisplayPopup(GameState.EventFindItem(1, 100));
                else if (evnt <= 8) // 10% chance to find gold
                    DisplayPopup(GameState.EventFindGold(1, 100));
                else if (evnt <= 10) // 10% chance to find stream
                    DisplayPopup(GameState.EventEncounterStream(), 1.5f);
                // 50% chance for no event
            }
            BonusChance = 0;
        }

        #endregion Events
    }
}