using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Entities;

public class CityScene : Control
{
    private Info info;
    private CharacterScene characterScene;

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
        info = (Info)GetNode("/root/Info");
        characterScene = (CharacterScene)GetNode("/root/CharacterScene");
        info.Scale = new Vector2(1, 1);
        characterScene.Scale = new Vector2(1, 1);
        info.DisplayStats();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}