using Godot;
using System;

public class CityScene : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var shit = (Info)GetNode("/root/Info");
        shit.Visible = true;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}