using Godot;
using Sulimn.Classes;

public class MainScene : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        OS.WindowMaximized = true;
        GameState.LoadAll();
    }

    private void _on_BtnNewHero_pressed()
    {
        GetTree().ChangeScene("scenes/NewHero.tscn");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}