using Godot;
using System;

public class InventoryScene : CanvasLayer
{
    private AnimationPlayer player;
    private bool showScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = (AnimationPlayer)GetNode("AnimationPlayer");
        SlideIn();
        this.Scale = new Vector2(0, 0);
    }

    #region Animation

    private void SlideIn() => player.Play("slide");

    private void SlideOut() => player.PlayBackwards("slide");

    #endregion Animation

    private void _on_Button_pressed()
    {
        if (!showScene)
        {
            SlideOut();
        }
        else
        {
            SlideIn();
        }
        showScene = !showScene;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}