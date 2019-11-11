using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Entities;

namespace Sulimn.Scenes.City
{
    public class CityScene : Node2D
    {
        private Player Player;
        private Vector2 PreviousPosition;

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
            {
                GetTree().ChangeScene("scenes/MainScene.tscn");
                GameState.CurrentHero = new Hero();
            }
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Player = (Player)GetTree().CurrentScene.FindNode("Player");
            GameState.Info.Scale = new Vector2(1, 1);
            GameState.Info.DisplayStats();
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

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _PhysicsProcess(float delta)
        {
        }
    }
}