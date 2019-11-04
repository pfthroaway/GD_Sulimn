using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Extensions;

public class FieldsScene : Node2D
{
    private CharacterScene characterScene;
    private Player Player;
    private Vector2 PreviousPosition;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Player = (Player)GetTree().CurrentScene.FindNode("Player");
        characterScene = (CharacterScene)GetNode("/root/CharacterScene");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        if (PreviousPosition != Player.GetGlobalPosition())
        {
            PreviousPosition = Player.GetGlobalPosition();

            if (characterScene.ShowScene)
                Player.Disabled = true;

            CheckForEvents();
        }
    }

    private void _on_CityArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
    {
        if (area is Node player && player.IsInGroup("Player"))
            GetTree().ChangeSceneTo(GameState.GoBack());
    }

    #region Events

    /// <summary>Check whether the an event happened on this move.</summary>
    private void CheckForEvents()
    {
        if (Functions.GenerateRandomNumber(1, 100) < 10)
        {
            GD.Print("This is where a battle would occur.");
            //GameState.AddSceneToHistory(GetTree().CurrentScene);
            //GameState.EventEncounterEnemy(1, 5);
            //GetTree().ChangeScene("res://scenes/battle/BattleScene.tscn");
        }
        else if (Functions.GenerateRandomNumber(1, 100) < 5)
        {
            GD.Print(GameState.EventFindItem(1, 300));
            characterScene.UpdateLabels();
        }
        else if (Functions.GenerateRandomNumber(1, 100) < 5)
        {
            GD.Print(GameState.EventFindGold(1, 200));
            characterScene.UpdateLabels();
        }
    }

    #endregion Events
}