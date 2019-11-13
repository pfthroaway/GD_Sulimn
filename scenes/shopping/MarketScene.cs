using Godot;
using Sulimn.Actors;
using Sulimn.Classes;

namespace Sulimn.Scenes.City
{
    public class MarketScene : Node2D
    {
        private Player Player;

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
                GetTree().ChangeSceneTo(GameState.GoBack());
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Player = (Player)GetTree().CurrentScene.FindNode("Player");
            GameState.Info.Scale = new Vector2(1, 1);
            GameState.Info.DisplayStats();
        }

        private void _on_WeaponsArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
            {
                Player.Move("down");
                GameState.AddSceneToHistory(GetTree().CurrentScene);
                GetTree().ChangeScene("res://scenes/shopping/WeaponsRUsScene.tscn");
            }
        }

        private void _on_CityArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
                GetTree().ChangeSceneTo(GameState.GoBack());
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _PhysicsProcess(float delta)
        {
        }
    }
}