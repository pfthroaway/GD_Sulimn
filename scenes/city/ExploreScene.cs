using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Extensions;

public class ExploreScene : Node2D
{
    private Player Player;
    private CharacterScene characterScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Player = (Player)GetTree().CurrentScene.FindNode("Player");
        Player.Position = new Vector2(GameState.HeroPosition);
        characterScene = (CharacterScene)GetNode("/root/CharacterScene");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        if (GameState.HeroPosition != Player.GlobalPosition)
        {
            GameState.HeroPosition = new Vector2(Player.Position);
            if (EncounterEnemy())
                GetTree().ChangeScene("res://scenes/battle/BattleScene.tscn");
        }

        if (characterScene.ShowScene)
            Player.Disabled = true;
    }

    private bool EncounterEnemy() => Functions.GenerateRandomNumber(1, 100) < 10;
}