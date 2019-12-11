using Godot;
using Sulimn.Actors;
using Sulimn.Classes;

namespace Sulimn.Scenes.City
{
    public class TavernScene : Node2D
    {
        private Player Player;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready() => Player = (Player)GetTree().CurrentScene.FindNode("Player");

        #region Areas Entered

        private void _on_BlackjackArea_area_entered(object area)
        {
            if (area is Node player && player.IsInGroup("Player"))
            {
                Player.Move("down");
                GameState.AddSceneToHistory(GetTree().CurrentScene);
                GetTree().ChangeScene("res://scenes/gambling/BlackjackScene.tscn");
            }
        }

        private void _on_CityArea_area_entered(object area) => GetTree().ChangeSceneTo(GameState.GoBack());

        #endregion Areas Entered
    }
}