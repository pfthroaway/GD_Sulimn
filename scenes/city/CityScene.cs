using Godot;
using Sulimn.Actors;
using Sulimn.Classes;
using Sulimn.Classes.Entities;

namespace Sulimn.Scenes.City
{
    public class CityScene : Node2D
    {
        private Player Player;

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
            GameState.Info.Scale = new Vector2(1, 1);
            GameState.Info.DisplayStats();
        }

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

        #endregion Areas Entered

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        //public override void _PhysicsProcess(float delta)
        //{
        //}
    }
}