using Godot;
using System;

public class MyAcceptDialog : AcceptDialog
{
    private Player player;
    private float timer = 0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = (Player)GetTree().CurrentScene.FindNode("Player");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        player.Disabled = Visible;
        if (Visible)
            timer += delta;
        if (timer > 1f)
        {
            Visible = false;
            timer = 0;
        }
    }
}