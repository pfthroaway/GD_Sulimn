using Godot;
using System;

public class CityScene : Control
{
    private Info info;
    private CharacterScene characterScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        info = (Info)GetNode("/root/Info");
        info.Visible = true;
        info.DisplayStats();
        characterScene = (CharacterScene)GetNode("/root/CharacterScene");
        characterScene.Visible = true;
        characterScene.UpdateLabels();
        //Control ctrl = (Control)characterScene.GetNode("Info");
        //GD.Print(ctrl.GetFocusMode());
        //ctrl.SetFocusMode(FocusModeEnum.All);
        //GD.Print(ctrl.GetFocusMode());
        //GD.Print(ctrl.HasFocus());
        //ctrl.GrabFocus();
        //GD.Print(ctrl.HasFocus());
        //characterScene.SetFocusMode(FocusModeEnum.All);
        //characterScene.GrabFocus();
        //characterScene.GrabClickFocus();
        //characterScene.
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}